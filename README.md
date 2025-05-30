# 🧠 Proyecto Centinela

**Proyecto Centinela** es un videojuego 2D en pixel art con perspectiva axonométrica, desarrollado en Unity. Está ambientado en una simulación virtual que evalúa el rendimiento del jugador, con integración completa de autenticación, gestión de sesiones y puntuaciones mediante una API RESTful en ASP.NET y base de datos MySQL.

## 🧾 Características principales

- Sistema de **login, registro y edición de perfil**.
- **Leaderboard competitivo** con puntuación automática por enemigo.
- Gestión de **sesiones activas** y panel de administración interno para usuarios con rol admin.
- **Combate en tiempo real** con armas intercambiables y munición regenerativa.
- Diseño modular y código mantenible basado en principios SOLID.
- Estética retro-futurista en pixel art.

## 🧩 Requisitos

| Elemento         | Requisito mínimo              |
|------------------|-------------------------------|
| SO               | Windows 10 o superior         |
| CPU              | Intel i5 o equivalente        |
| RAM              | 4 GB (8 GB recomendado)       |
| Espacio en disco | 500 MB                        |
| Dependencias     | Unity 6, .NET SDK 7+, XAMPP   |

> 🔐 Código secreto de administrador: `1` *(modificable)*

## ⚙️ Instalación y ejecución

1. Clona este repositorio o descarga como `.zip`.
2. Abre la solución backend en:
 `Backend/ProyectoCentinela/ProyectoCentinela.sln`
3. Ejecuta el proyecto (`program.cs`) desde tu IDE (.NET SDK 7+ requerido).
4. Asegúrate de que MySQL está activo en XAMPP y ejecuta el script SQL proporcionado para crear la base de datos en `Backend/pryoecto_centinela.sql`.

---

### 🧰 Configuración de XAMPP (o servicio MySQL equivalente)

Este proyecto requiere una instancia de MySQL activa. Para ello, puedes usar **XAMPP** u otra solución similar.

#### ▶️ Pasos básicos con XAMPP:

1. Descarga e instala XAMPP desde:  
   👉 [https://www.apachefriends.org/es/index.html](https://www.apachefriends.org/es/index.html)
2. Abre el **Panel de Control de XAMPP**.
3. Activa el servicio **MySQL** (botón "Start").
4. Accede a `http://localhost/phpmyadmin` y crea la base de datos ejecutando el script `.sql` proporcionado en este proyecto.

> 💡 Si usas otro gestor como Laragon, WAMP o MySQL nativo, asegúrate de usar los mismos datos de conexión configurados en el archivo de la API.

---

5. Abre el proyecto en Unity 6 y ejecuta la escena `LoginScene`.

## 🎮 Gameplay

El jugador explora un entorno simulado, recoge armas y derrota enemigos para obtener puntos. La puntuación se registra en el leaderboard si está en modo competitivo. Los elementos del entorno son dinámicos y destructibles, con mapas generados aleatoriamente a partir de plantillas.

| Acción                | Tecla             |
|-----------------------|------------------|
| Moverse               | WASD / Flechas   |
| Recoger objeto/arma   | E                |
| Cambiar arma equipada | TAB              |
| Salir al menú         | ESC              |

## 🧑‍💻 Funcionalidades por rol actuales

### 👤 Usuario:
- Acceder al juego competitivo
- Consultar y editar su perfil
- Ver leaderboard

### 🛠️ Administrador:
- Editar/eliminar cualquier usuario
- Visualizar todos los usuarios y sus sesiones (conectados, no conectados y registros sin login)

## 🖼 Interfaz de usuario

- Inputs claros para nombre de usuario, email y contraseña
- Botones grandes y visibles
- Indicadores visuales para sesiones activas (iconos verdes)
- Leaderboard con puntuación resaltada en verde
- Diseño responsivo y adaptado a distintas resoluciones

## 📂 Estructura del menú principal

- **Jugar**: modo historia (próximamente)
- **Competitivo**: modo actual con registro de puntuaciones
- **Perfil**: edición de nombre, email, contraseña, eliminar cuenta
- **Sesiones**: gestión de usuarios y sesiones activas
- **Leaderboard**: clasificación por puntuación
- **Créditos** y **Salir**

## 📌 Notas

Este proyecto ha sido desarrollado como parte de un proyecto final del ciclo de Desarrollo de Aplicaciones Multiplataforma (DAM). Está diseñado para servir tanto como producto jugable como ejemplo educativo de arquitectura cliente-servidor aplicada a videojuegos.

---

> ¿Dudas o sugerencias? Abre una issue o contribuye al repositorio.
