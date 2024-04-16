# MeuIFMG - Discord bot

## Resumo
Projeto feito em .Net 6 e Discord.Net para um bot no Discord que exibe as notas do usuário diretamente do site [MeuIFMG](meu.ifmg.edu.br)

Continuação do antigo projeto [Meu-IFMG-WebScrapping](https://github.com/KindaSnoowy/Meu-IFMG-WebScrapping), anteriormente feito inteiramente em Python e menos completo.

## Uso
!register -> Inicia a tarefa de registro pela DM do usuário.
!forcesync -> Sincroniza as notas do banco de dados com as do MeuIFMG
!notas -> Inicia o menu das Notas

## Explicação

- Ponte entre o Python e o C#
Em [pythonScripts](https://github.com/KindaSnoowy/meuifmg-discord/tree/main/pythonScripts) maincall.py pode ser executado pelo terminal com o comando `python maincall.py <ra> <senha>` que printa diretamente o json carregando as notas do usuário requisitado. A partir daí [pythonCaller.cs](https://github.com/KindaSnoowy/meuifmg-discord/blob/main/modules/pythonCaller.cs) é o responsável por executar um processo dentro do .net e retornar a string para o código.

## Setup
1) Baixe o .Net 6 e o Python em sua última versão.
2) Baixe as bibliotecas do Python em sua máquina (não no ambiente do Visual Studio Code)

   `pip install beautifulsoup4`
   `pip install requests`

3) [Crie um bot](https://discord.com/developers/docs/getting-started#step-1-creating-an-app) e troque a varíavel Token em Program.cs pelo Token de seu bot.

   ```
   _client.Log += LogTask;

   var token = "Token aqui!! XD"; <--

   await _client.LoginAsync(TokenType.Bot, token);
   ```

4) [Convide seu bot para um servidor](https://discord.com/developers/docs/getting-started#adding-scopes-and-bot-permissions)
5) E é isso (eu acho)

## To-do
-> Sincronização automática periodica com o MeuIFMG (atualmente é somente manual pelo !forcesync).
-> Notificação em alteração de notas.
