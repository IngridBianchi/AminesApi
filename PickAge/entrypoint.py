import subprocess
from datetime import datetime

print(f"[{datetime.now().strftime('%H:%M:%S')}] 🚀 Iniciando verificación y consumidor PickAge")

try:
    subprocess.run(["python", "verify_rabbitmq.py"], check=True)
    print(f"[{datetime.now().strftime('%H:%M:%S')}] ✅ Verificación completada")
    subprocess.run(["python", "AgeSelector.py"], check=True)
except subprocess.CalledProcessError as e:
    print(f"[ERROR] Falló un script → {e}")
