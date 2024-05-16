# IntelX Reader

## ¿Qué es IntelX Reader?

IntelX Reader es una aplicación de escritorio diseñada para ayudar a los usuarios a buscar y extraer información específica de archivos de texto contenidos dentro de directorios. Esta herramienta es especialmente útil para analizar grandes volúmenes de datos y encontrar información sensible de manera rápida y eficiente.

## Características Principales

- **Selección de Directorio**: Permite al usuario seleccionar un directorio que contiene varios archivos comprimidos y subdirectorios.
- **Búsqueda de Archivos Específicos**: Busca archivos de texto específicos, como `Passwords.txt`, dentro de todos los subdirectorios del directorio seleccionado.
- **Extracción de Información**: Extrae información específica de los archivos encontrados, como URLs, nombres de usuario y contraseñas.
- **Guardado de Resultados**: Guarda los resultados en archivos de texto organizados por fecha en una carpeta denominada `Resultados`.
- **Interfaz Intuitiva**: Proporciona una interfaz gráfica de usuario (GUI) sencilla y fácil de usar.
- **Barra de Progreso**: Muestra el progreso de la búsqueda y extracción de información.
- **Historial de Archivos Procesados**: Permite visualizar un historial de los archivos procesados organizados por fecha y abrirlos directamente desde la aplicación.

## ¿Quién puede beneficiarse?

- **Investigadores de Seguridad**: Para analizar grandes volúmenes de datos en busca de información sensible.
- **Analistas de Datos**: Para extraer información relevante de conjuntos de datos grandes y desorganizados.
- **Usuarios Avanzados**: Para mantener registros organizados y acceder rápidamente a información específica dentro de archivos de texto.

## Cómo Usar IntelX Reader

1. **Abrir la Aplicación**: Inicie IntelX Reader desde su computadora.
2. **Seleccionar un Directorio**: Utilice el menú `Archivo` para seleccionar un directorio que desea analizar.
3. **Buscar Información**: Ingrese el texto de interés en el cuadro correspondiente y haga clic en `Iniciar`.
4. **Ver Progreso**: Observe la barra de progreso y el contador de resultados mientras la aplicación procesa los archivos.
5. **Ver Resultados**: Una vez completado, puede ver y abrir los resultados en la sección `Archivos Procesados` del menú `Ver`.
6. **Abrir Resultados**: Haga clic en cualquier archivo en el historial para abrirlo y ver el contenido extraído.

IntelX Reader facilita la tarea de encontrar y organizar información sensible en archivos de texto, proporcionando una solución eficiente y fácil de usar para profesionales y usuarios avanzados.

## Tecnologías Utilizadas

- **Lenguaje de Programación**: C#
- **Entorno de Desarrollo**: Visual Studio 2022
- **Framework**: .NET Framework 4.7.2
- **Componentes de la Interfaz de Usuario**: Windows Forms

## Cómo Compilar el Proyecto

1. **Clonar el repositorio**:
    ```sh
    https://github.com/JamilSec/ReadIntelX.git
    ```
2. **Abrir el proyecto en Visual Studio 2022**:
    - Seleccionar `Archivo > Abrir > Proyecto/Solución`.
    - Navegar hasta la carpeta donde clonaste el repositorio y seleccionar el archivo `.sln`.
3. **Restaurar los paquetes NuGet**:
    - En el explorador de soluciones, hacer clic derecho sobre la solución y seleccionar `Restaurar paquetes NuGet`.
4. **Compilar el proyecto**:
    - Hacer clic en `Compilar > Compilar Solución`.

## Cómo Contribuir

1. **Fork el repositorio**
2. **Crear una rama feature** (`git checkout -b feature/AmazingFeature`)
3. **Hacer commit de los cambios** (`git commit -m 'Add some AmazingFeature'`)
4. **Hacer push a la rama** (`git push origin feature/AmazingFeature`)
5. **Abrir un Pull Request**

¡Gracias por usar IntelX Reader!
