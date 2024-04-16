from bs4 import BeautifulSoup
import re

## TRATA O RETORNO DO SITE DA ESCOLA
def scraphtml(html):
    soup = BeautifulSoup(html, 'html.parser')

    etapas = []
    for i in range(6):
        etapas.append(soup.find(id=f'ctl24_PanelEtapa{i}'))

    notas = {
        "Primeira Etapa": {},
        "Primeira Etapa (Recuperação)": {},
        "Segunda Etapa": {},
        "Segunda Etapa (Recuperação)": {},
        "Terceira Etapa": {},
        "Exame Final": {},
    }

    re_pattern = r'^(.*)\s*-\s*Situação:\s*(.*)$'
    for index, (k, v) in enumerate(notas.items()):
        materias = etapas[index].find_all(class_='EduLabel')
        for i in materias:
            match = re.match(re_pattern, i.text)
            if match:
                v[match.group(1).strip()] = {"Situação": match.group(2).strip(), 
                                            "Element": i}

    for i, v in notas.items():
        for k, x in v.items():
            if x["Element"].find_next_sibling().text.strip() == "Nenhuma avaliação cadastrada.":
                x["Avaliações"] = "Nenhuma avaliação cadastrada."
            else:
                soup = x["Element"].find_next_sibling()
                headers = [header.get_text() for header in soup.find('tr', class_='EduGridHeader').find_all('td')]
                rows = soup.find_all('tr')[1:]
                data = []

                for row in rows:
                    row_data = {}
                    cells = row.find_all('td')
                    for index, cell in enumerate(cells):
                        row_data[headers[index]] = cell.get_text().strip()
                    data.append(row_data)

                del data[-1] ## Apaga último valor que é os somatórios do site
                for i in data:
                    if i["Valor da Avaliação"]:
                        i["Valor da Avaliação"] = float(i["Valor da Avaliação"].replace(",", "."))
                    if i["Nota Obtida"]:
                        i["Nota Obtida"] = float(i["Nota Obtida"].replace(",", "."))
                somatorios = []
                for row in soup.find_all('tr', align='right'):
                    for cell in row.find_all('td'):
                        if 'Somatório:' in cell.get_text():
                            match = re.match(r'Somatório:\s*([\d,]+)', cell.get_text())
                            somatorios.append(match.group(1))

                try:            
                    x["Somatórios"] = {"SomatórioTotal": float(somatorios[0].replace(",", ".")),
                                       "SomatórioAtual": float(somatorios[1].replace(",", "."))}
                except IndexError:       ## Em alguns casos, o somatório das notas obtidas é simplesmente nulo, mais fácil
                    somatorios.append(0.0)  ## um try except pra evitar erros.
                    

                x["Avaliações"] = data

    for _, v in notas.items():
        for _, x in v.items():
            if x["Element"]:
                del x["Element"]

    #print(notas["Exame Final"])
    return notas