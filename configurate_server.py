import socket
import json 

with open("PastryServer\\appsettings.json", "r", encoding="utf-8-sig") as f:
  data = json.load(f)
  
print(f"{socket.gethostbyname(socket.gethostname())}")
data["Kestrel"]["Endpoints"]["Https"]["Url"] = f"https://{socket.gethostbyname(socket.gethostname())}:5001"
print(data)

with open("PastryServer\\appsettings.json", "w", encoding="utf-8-sig") as f:
    json.dump(data, f, indent=4)

      
print("\n")  
with open("PastryServer\\Properties\\launchSettings.json", "r", encoding="utf-8-sig") as f:
  data = json.load(f)
print(data)

data["iisSettings"]["iisExpress"]["applicationUrl"] = f"http//{socket.gethostbyname(socket.gethostname())}:5000"
temp = data["profiles"]["http"]["applicationUrl"]
temp = temp.split(";")
temp[0] = f"http//{socket.gethostbyname(socket.gethostname())}:5000"
temp[1] = f"https//{socket.gethostbyname(socket.gethostname())}:5001"
temp = ';'.join(temp)
data["profiles"]["http"]["applicationUrl"] = temp

with open("PastryServer\\Properties\\launchSettings.json", "w", encoding="utf-8-sig") as f:
    json.dump(data, f, indent=2)