# TerminalServiceFT

[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.5.1-blue.svg)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)](https://www.microsoft.com/windows)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

**TerminalServiceFT** é um utilitário de monitoramento de sessões RDP (Remote Desktop Protocol) para Windows que permite listar e obter informações detalhadas sobre todas as sessões de Terminal Services ativas em um servidor.

## 📋 Índice

- [Características](#-características)
- [Pré-requisitos](#-pré-requisitos)
- [Instalação](#-instalação)
- [Como Usar](#-como-usar)
- [Exemplo de Saída](#-exemplo-de-saída)
- [Arquitetura do Projeto](#-arquitetura-do-projeto)
- [Informações Coletadas](#-informações-coletadas)
- [Casos de Uso](#-casos-de-uso)
- [Requisitos de Permissões](#-requisitos-de-permissões)
- [Limitações Conhecidas](#-limitações-conhecidas)
- [Contribuindo](#-contribuindo)
- [Licença](#-licença)

## ✨ Características

- 🖥️ **Monitoramento em Tempo Real**: Lista todas as sessões RDP ativas e desconectadas
- 🌐 **Detecção de IP**: Identifica o endereço IP de cada cliente conectado
- 👤 **Informações de Usuário**: Extrai nome de usuário e domínio
- 📺 **Configurações de Display**: Captura resolução de tela e profundidade de cor
- 🔍 **Estado da Sessão**: Mostra se a sessão está ativa, conectada, desconectada, etc.
- 💻 **Interface de Console**: Simples e direto, ideal para scripts e automação

## 🔧 Pré-requisitos

- **Sistema Operacional**: Windows Server 2008 R2 ou superior / Windows 7 ou superior
- **.NET Framework**: 4.5.1 ou superior
- **Permissões**: Privilégios de Administrador
- **Serviços**: Terminal Services / Remote Desktop Services habilitado

## 📥 Instalação

### Compilação Manual

1. Clone o repositório:
```bash
git clone https://github.com/seu-usuario/TerminalServiceHostConnect.git
cd TerminalServiceHostConnect
```

2. Compile usando Visual Studio:
   - Abra `TerminalServiceFT.sln` no Visual Studio
   - Pressione `F6` ou vá em **Build > Build Solution**

3. Ou compile via linha de comando:
```bash
msbuild TerminalServiceFT\TerminalServiceFT.csproj /p:Configuration=Release
```

## 🚀 Como Usar

### Execução Básica

1. **Execute como Administrador** (obrigatório):
   - Clique com botão direito em `TerminalServiceFT.exe`
   - Selecione "Executar como administrador"

2. Ou via PowerShell/CMD como Administrador:
```powershell
cd caminho\para\o\executavel
.\TerminalServiceFT.exe
```

3. O programa exibirá informações de todas as sessões ativas e aguardará uma tecla para encerrar.

### Uso em Scripts

```powershell
# Redirecionar saída para arquivo
.\TerminalServiceFT.exe > sessoes_rdp.txt

# Usar em scripts de monitoramento
$output = & ".\TerminalServiceFT.exe"
# Processar $output conforme necessário
```

## 📊 Exemplo de Saída

```
Session ID : 0
Session State : WTSListen
Workstation Name : 
IP Address : 0.0.0.0
User Name : \
Client Display Resolution: 0 x 0
Client Display Colour Depth: 0
Client Application Directory: 
-----------------------
Session ID : 1
Session State : WTSActive
Workstation Name : DESKTOP-CLIENTE
IP Address : 192.168.1.100
User Name : EMPRESA\joao.silva
Client Display Resolution: 1920 x 1080
Client Display Colour Depth: 8
Client Application Directory: C:\Windows\System32
-----------------------
Session ID : 2
Session State : WTSDisconnected
Workstation Name : LAPTOP-RH
IP Address : 192.168.1.105
User Name : EMPRESA\maria.santos
Client Display Resolution: 1366 x 768
Client Display Colour Depth: 8
Client Application Directory: C:\Windows\System32
-----------------------
```

## 🏗️ Arquitetura do Projeto

### Estrutura de Arquivos

```
TerminalServiceFT/
├── Program.cs              # Ponto de entrada e lógica principal
├── RDPDLL.cs              # Wrapper para WTS API (wtsapi32.dll)
├── usuarios.cs            # Modelo de dados para usuários
├── tsUtils.cs             # Utilitários auxiliares
└── Properties/
    └── AssemblyInfo.cs    # Metadados do assembly
```

### Componentes Principais

#### **Program.cs**
Contém o método `Main()` que:
- Enumera todas as sessões usando `WTSEnumerateSessions`
- Itera sobre cada sessão para coletar informações
- Exibe os dados formatados no console

#### **RDPDLL.cs**
Wrapper C# para a API nativa do Windows (`wtsapi32.dll`):
- **Importações DLL**: `WTSEnumerateSessions`, `WTSQuerySessionInformation`, `WTSFreeMemory`
- **Estruturas**: `WTS_SESSION_INFO`, `WTS_CLIENT_ADDRESS`, `WTS_CLIENT_DISPLAY`
- **Enumerações**: `WTS_CONNECTSTATE_CLASS`, `WTS_INFO_CLASS`

#### **usuarios.cs**
Classe modelo com propriedades para armazenar dados de sessão:
- `SUserName` - Nome do usuário
- `SDomain` - Domínio
- `SIPAddress` - Endereço IP
- `SClientApplicationDirectory` - Diretório do cliente

#### **tsUtils.cs**
Classe utilitária com método `retornausuarios()` - implementação alternativa da lógica de enumeração.

## 📝 Informações Coletadas

Para cada sessão ativa, o programa coleta:

| Informação | Descrição |
|------------|-----------|
| **Session ID** | Identificador único da sessão |
| **Session State** | Estado atual (Active, Connected, Disconnected, Idle, etc.) |
| **Workstation Name** | Nome da máquina cliente |
| **IP Address** | Endereço IPv4 do cliente remoto |
| **User Name** | Usuário no formato DOMÍNIO\usuário |
| **Display Resolution** | Resolução da tela do cliente (largura x altura) |
| **Color Depth** | Profundidade de cor (4, 8, 15, 16, 24 bits) |
| **Application Directory** | Diretório de aplicação do cliente |

### Estados de Sessão Possíveis

- `WTSActive` - Sessão ativa e em uso
- `WTSConnected` - Conectada
- `WTSConnectQuery` - Em processo de conexão
- `WTSDisconnected` - Desconectada mas não encerrada
- `WTSIdle` - Ociosa
- `WTSListen` - Aguardando conexões
- `WTSReset` - Em reset
- `WTSDown` - Inativa
- `WTSInit` - Inicializando

## 💼 Casos de Uso

### 1. **Monitoramento de Servidor**
Administradores podem verificar rapidamente quem está conectado ao servidor RDP.

### 2. **Auditoria de Segurança**
Registrar logs de conexões para conformidade e segurança:
```powershell
.\TerminalServiceFT.exe >> logs\rdp_audit_$(Get-Date -Format 'yyyyMMdd').log
```

### 3. **Diagnóstico de Suporte**
Identificar problemas de conexão e configurações de cliente.

### 4. **Automação e Alertas**
Integrar em scripts de monitoramento para alertas automáticos:
```powershell
# Exemplo: Alertar se mais de X usuários conectados
$sessoes = & ".\TerminalServiceFT.exe"
$count = ($sessoes | Select-String "WTSActive").Count
if ($count -gt 10) {
    Send-Alert "Muitas sessões RDP ativas: $count"
}
```

### 5. **Relatórios de Uso**
Gerar relatórios periódicos de uso do servidor RDP.

## 🔐 Requisitos de Permissões

### Privilégios Necessários
- ✅ **Administrador Local**: Obrigatório para acessar a WTS API
- ✅ **Acesso ao wtsapi32.dll**: Disponível nativamente no Windows

### Execução sem Privilégios
❌ A aplicação **NÃO funcionará** sem privilégios administrativos e retornará erro ou informações limitadas.

## ⚠️ Limitações Conhecidas

1. **Somente IPv4**: Atualmente extrai apenas endereços IPv4 (bytes 2-5 do array)
2. **Console Only**: Interface apenas em linha de comando
3. **Servidor Local**: Conecta ao servidor local (pode ser adaptado para remoto)
4. **Sessões Console**: Sessão console (ID 0) pode não ter informações completas
5. **Bloqueio de Tela**: Aguarda `Console.ReadKey()` ao final (não ideal para automação sem interação)
