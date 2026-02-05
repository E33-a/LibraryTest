# Library Manager - Sistema de Gestión Bibliotecaria

## Requisitos
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Ranorex Studio](https://www.ranorex.com/download/) (para pruebas automatizadas)
- Windows 10/11

## Instalación rápida

```bash
# 1. Clonar repositorio
git clone https://github.com/tu-usuario/library-manager.git
cd library-manager

# 2. Restaurar paquetes NuGet
dotnet restore src/LibraryManager.sln

# 3. Compilar
dotnet build src/LibraryManager.sln

# 4. Ejecutar aplicación
dotnet run --project src/LibraryManager.Desktop

# 5. Verificar
# Abre navegador o espera ventana WPF
# Login: admin / pass123
