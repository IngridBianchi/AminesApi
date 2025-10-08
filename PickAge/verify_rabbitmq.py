import os
import time
from datetime import datetime
import pika
from dotenv import load_dotenv

# Cargar variables de entorno
load_dotenv()

host = os.getenv('RABBITMQ_HOST')
username = os.getenv('RABBITMQ_USERNAME')
password = os.getenv('RABBITMQ_PASSWORD')
queue_name = os.getenv('RABBITMQ_QUEUE_NAME')

# Validar configuración
if not all([host, username, password, queue_name]):
    raise ValueError("❌ Faltan variables de entorno para conectar a RabbitMQ")

print(f"[{datetime.now().strftime('%H:%M:%S')}] 🔍 Verificando conexión a RabbitMQ → host={host}, queue={queue_name}")

# Configurar conexión
credentials = pika.PlainCredentials(username, password)
params = pika.ConnectionParameters(host=host, credentials=credentials)

# Intentar conexión con reintentos
for attempt in range(10):
    try:
        connection = pika.BlockingConnection(params)
        print(f"[{datetime.now().strftime('%H:%M:%S')}] ✅ Conexión exitosa en intento {attempt + 1}")
        break
    except pika.exceptions.AMQPConnectionError as e:
        print(f"[{datetime.now().strftime('%H:%M:%S')}] ⏳ Intento {attempt + 1}: RabbitMQ no disponible → {e}")
        time.sleep(5)
else:
    raise Exception("❌ No se pudo conectar a RabbitMQ después de varios intentos.")

# Verificar existencia de la cola
channel = connection.channel()
channel.queue_declare(queue=queue_name, durable=True)
print(f"[{datetime.now().strftime('%H:%M:%S')}] 📦 Cola '{queue_name}' verificada o creada correctamente")

connection.close()
print(f"[{datetime.now().strftime('%H:%M:%S')}] 🟢 Verificación completa. PickAge puede iniciar.")
