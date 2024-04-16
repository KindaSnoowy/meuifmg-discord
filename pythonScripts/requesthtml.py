import requests
from bs4 import BeautifulSoup

## LOGA NO SITE DA ESCOLA PRO REQUEST FUNCIONAR
def requesthtml(user, password):
    with requests.Session() as s:
        headers = {"content-type": "application/x-www-form-urlencoded", "user-agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246"}
        
        ## Get keys
        getkeys = requests.get("https://meu.ifmg.edu.br/Corpore.Net/Login.aspx", headers=headers)

        soup = BeautifulSoup(getkeys.text, 'html.parser')
        entrykeys = {
            "__VIEWSTATE":soup.find('input', {'id': '__VIEWSTATE'})["value"],
            "__VIEWSTATEGENERATOR":soup.find('input', {'id': '__VIEWSTATEGENERATOR'})["value"],
            "__EVENTVALIDATION":soup.find('input', {'id': '__EVENTVALIDATION'})["value"],
        }
        
        ## Login post
        s.post(headers=headers, data=
                {"__VIEWSTATE": entrykeys["__VIEWSTATE"],
                "__VIEWSTATEGENERATOR": entrykeys["__VIEWSTATEGENERATOR"],
                "__EVENTVALIDATION": entrykeys["__EVENTVALIDATION"],
                "txtUser": user, 
                "txtPass": password,
                "btnLogin": "Acessar"}, url="https://meu.ifmg.edu.br/Corpore.Net/Login.aspx")

        ## Get last rdContexto
        getcontext = s.get(headers=headers, url="https://meu.ifmg.edu.br/Corpore.Net/Source/Edu-Educacional/RM.EDU.CONTEXTO/EduSelecionarContextoModalWebForm.aspx?Qs=ActionID%3dEduNotaAvaliacaoActionWeb%26SelectedMenuIDKey%3dmnNotasAval")
        soup = BeautifulSoup(getcontext.text, 'html.parser')

        rdcontextos = soup.find_all(id="rdContexto")
        rdcontext = rdcontextos[-1]['value']

        entrykeys["__VIEWSTATE"] = soup.find('input', {'id': '__VIEWSTATE'})["value"],

        ## Set context
        s.post(headers=headers, cookies=s.cookies, data={
            "__VIEWSTATE": entrykeys["__VIEWSTATE"],
            "__VIEWSTATEGENERATOR": entrykeys["__VIEWSTATEGENERATOR"],
            "__EVENTVALIDATION": entrykeys["__EVENTVALIDATION"],
            "rdContexto": rdcontext,
        }, url="https://meu.ifmg.edu.br/Corpore.Net/Source/Edu-Educacional/RM.EDU.CONTEXTO/EduSelecionarContextoModalWebForm.aspx")

        ## Get notas
        r = s.get(headers=headers, url="https://meu.ifmg.edu.br/Corpore.Net/Main.aspx?ActionID=EduNotaAvaliacaoActionWeb&SelectedMenuIDKey=mnNotasAval")
        
        return r.text