import os
import asyncio
from datetime import datetime
from dotenv import load_dotenv
import pika
import json

# Cargar variables de entorno desde .env
load_dotenv()

# Obtener las credenciales de RabbitMQ desde .env
host = os.getenv('RABBITMQ_HOST')
username = os.getenv('RABBITMQ_USERNAME')
password = os.getenv('RABBITMQ_PASSWORD')
queue_name = os.getenv('RABBITMQ_QUEUE_NAME')

# Función para procesar mensajes
def callback(ch, method, properties, body):
    message = body.decode('utf-8')
    print(f"Mensaje recibido: {message}")
    # Parsear la cadena manualmente en lugar de JSON
    parts = message.split(', ')
    data = {}
    for part in parts:
        key_value = part.split(': ')
        if len(key_value) == 2:
            data[key_value[0].lower()] = key_value[1]
    # Extraer birthyear y validar
    birthyear = data.get('birthyear')
    if birthyear and birthyear.isdigit() and int(birthyear) >= 18:
        print(f"Usuario {data.get('name')} es Adulto (birthyear: {birthyear})")
    else:
        print(f"Usuario {data.get('name')} es Niño (birthyear: {birthyear})")
    ch.basic_ack(delivery_tag=method.delivery_tag)  # Confirmar el mensaje

async def main():
    # Configurar la conexión a RabbitMQ
    credentials = pika.PlainCredentials(username, password)
    connection_params = pika.ConnectionParameters(host=host, credentials=credentials)
    connection = pika.BlockingConnection(connection_params)
    channel = connection.channel()

    # Declarar la cola
    channel.queue_declare(queue=queue_name, durable=False)

    # Configurar el consumidor
    channel.basic_consume(queue=queue_name, on_message_callback=callback)

    print(f"Esperando mensajes en la cola {queue_name}. Para salir, presiona CTRL+C")
    channel.start_consuming()

if __name__ == "__main__":
    asyncio.run(main())