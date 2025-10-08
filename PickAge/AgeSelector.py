from datetime import datetime
print(f"[{datetime.now().strftime('%H:%M:%S')}] 🧭 Iniciando AgeSelector.py", flush=True)

import os
import time
import threading
import socket
from dotenv import load_dotenv
import pika

# 🔧 Cargar variables de entorno
load_dotenv()
host = os.getenv('RABBITMQ_HOST')
username = os.getenv('RABBITMQ_USERNAME')
password = os.getenv('RABBITMQ_PASSWORD')
queue_name = os.getenv('RABBITMQ_QUEUE_NAME')
port = int(os.getenv('RABBITMQ_PORT', 5672))

if not all([host, username, password, queue_name]):
    raise ValueError("❌ Faltan variables de entorno para conectar a RabbitMQ")

# ❤️ Latido para monitoreo
def heartbeat():
    while True:
        print(f"[{datetime.now().strftime('%H:%M:%S')}] ❤️ PickAge sigue vivo")
        time.sleep(30)

# 📦 Procesamiento de mensajes
def callback(ch, method, properties, body):
    try:
        message = body.decode('utf-8')
        print(f"[{datetime.now().strftime('%H:%M:%S')}] 📩 Mensaje recibido: {message}")

        parts = message.split(', ')
        data = {}
        for part in parts:
            key_value = part.split(': ')
            if len(key_value) == 2:
                data[key_value[0].lower()] = key_value[1]

        birthyear = data.get('birthyear')
        name = data.get('name', 'Desconocido')
        routing_key = 'child'  # default

        if birthyear and birthyear.isdigit():
            age = datetime.now().year - int(birthyear)
            routing_key = 'adult' if age >= 18 else 'child'
            print(f"→ Usuario {name} tiene {age} años → clasificado como '{routing_key}'")
        else:
            print(f"→ Usuario {name} tiene un birthyear inválido → clasificado como 'child'")

        ch.basic_publish(
            exchange='usuarios_topic',
            routing_key=routing_key,
            body=body
        )
        print(f"[{datetime.now().strftime('%H:%M:%S')}] 📤 Mensaje reenviado con routing_key='{routing_key}'")
        ch.basic_ack(delivery_tag=method.delivery_tag)

    except Exception as e:
        print(f"[ERROR] Falló el procesamiento del mensaje → {type(e).__name__}: {e}")
        ch.basic_nack(delivery_tag=method.delivery_tag)

# 🔌 Conexión con RabbitMQ con manejo de errores
def connect_to_rabbitmq():
    credentials = pika.PlainCredentials(username, password)
    params = pika.ConnectionParameters(host=host, port=port, credentials=credentials)
    for attempt in range(15):
        try:
            print(f"[{datetime.now().strftime('%H:%M:%S')}] 🔍 Intento {attempt + 1}: Conectando a {host}:{port}")
            connection = pika.BlockingConnection(params)
            print(f"[{datetime.now().strftime('%H:%M:%S')}] ✅ Conexión establecida en intento {attempt + 1}")
            return connection
        except (pika.exceptions.AMQPConnectionError, socket.gaierror) as e:
            print(f"[{datetime.now().strftime('%H:%M:%S')}] ⏳ Intento {attempt + 1}: Error de conexión → {type(e).__name__}: {e}")
            time.sleep(10)
    raise Exception("❌ No se pudo conectar a RabbitMQ después de varios intentos.")

# 🚀 Inicio del consumidor
def main():
    print(f"[{datetime.now().strftime('%H:%M:%S')}] 🧭 Iniciando consumidor PickAge")
    threading.Thread(target=heartbeat, daemon=True).start()

    try:
        connection = connect_to_rabbitmq()
        channel = connection.channel()
        channel.exchange_declare(exchange='usuarios_topic', exchange_type='topic', durable=True)
        channel.queue_declare(queue=queue_name, durable=True)
        channel.queue_bind(exchange='usuarios_topic', queue=queue_name, routing_key='member')
        print(f"[{datetime.now().strftime('%H:%M:%S')}] 📦 Cola '{queue_name}' declarada y vinculada a 'usuarios_topic' con routing_key='member'")
        channel.basic_consume(queue=queue_name, on_message_callback=callback)
        print(f"[{datetime.now().strftime('%H:%M:%S')}] 🟢 Esperando mensajes en la cola '{queue_name}'")
        channel.start_consuming()

    except KeyboardInterrupt:
        print(f"[{datetime.now().strftime('%H:%M:%S')}] 🛑 Interrupción manual. Cerrando consumidor.")
    except Exception as e:
        print(f"[ERROR] Falló el consumidor PickAge → {type(e).__name__}: {e}")

if __name__ == "__main__":
    main()
