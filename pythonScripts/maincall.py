import sys
import requesthtml
import scraphtml
import json

def getNotas(user, password):
    html = requesthtml.requesthtml(user, password)
    notas = scraphtml.scraphtml(html)

    return notas

if __name__ == "__main__":
    if len(sys.argv) != 3:
        print("Uso incorreto: python getNotas.py <user> <password>")
        sys.exit(1)

    user = sys.argv[1]
    password = sys.argv[2]

    notas = getNotas(user, password)
    print(json.dumps(notas))