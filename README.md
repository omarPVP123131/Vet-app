# Sistema de Gestión Veterinaria 🐾

Aplicación de escritorio desarrollada en WPF (Windows Presentation Foundation) para la gestión integral de pacientes en clínicas veterinarias. Esta aplicación permite administrar eficientemente la información de mascotas, sus dueños y las consultas médicas.

## Características Principales

- **Gestión de Pacientes**
  - Registro completo de mascotas
  - Historial médico detallado
  - Seguimiento de vacunaciones
  - Registro de tratamientos

- **Gestión de Propietarios**
  - Datos de contacto
  - Historial de visitas
  - Registro de pagos

- **Gestión de Citas**
  - Calendario de consultas
  - Recordatorios automáticos
  - Estado de citas

## Tecnologías Utilizadas

- **Frontend**: WPF (Windows Presentation Foundation)
- **Backend**: C#
- **Base de Datos**: SQLite
- **.NET Framework**: [Especificar versión]

## Requisitos del Sistema

- Windows 7 o superior
- .NET Framework [versión]
- Mínimo 4GB de RAM
- 100MB de espacio en disco duro

## Instalación

1. Descarga el instalador desde la sección de releases
2. Ejecuta el archivo .exe
3. Sigue las instrucciones del asistente de instalación
4. La base de datos se creará automáticamente en la primera ejecución

## Configuración de la Base de Datos

La aplicación utiliza SQLite como sistema de gestión de base de datos. La base de datos se crea automáticamente en:

```
C:\Users\[Usuario]\AppData\Local\VeterinaryApp\database.db
```

## Uso

1. Inicia la aplicación desde el acceso directo creado en el escritorio
2. Accede con tus credenciales de usuario
3. Navega por el menú principal para acceder a las diferentes funcionalidades

## Estructura del Proyecto

```
VeterinaryApp/
│
├── Models/
│   ├── Patient.cs
│   ├── Owner.cs
│   └── Appointment.cs
│
├── Views/
│   ├── MainWindow.xaml
│   ├── PatientView.xaml
│   └── AppointmentView.xaml
│
├── ViewModels/
│   ├── PatientViewModel.cs
│   └── AppointmentViewModel.cs
│
└── Services/
    ├── DatabaseService.cs
    └── PrintService.cs
```

## Contribución

Si deseas contribuir al proyecto:

1. Haz un Fork del repositorio
2. Crea una nueva rama (`git checkout -b feature/AmazingFeature`)
3. Realiza tus cambios
4. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
5. Push a la rama (`git push origin feature/AmazingFeature`)
6. Abre un Pull Request

## Licencia

Este proyecto está bajo la licencia [especificar licencia].

## Contacto

[Tu Nombre] - [tu@email.com]

Link del proyecto: [https://github.com/username/repo]

## Agradecimientos

- [Biblioteca/Recurso 1]
- [Biblioteca/Recurso 2]
- [Biblioteca/Recurso 3]