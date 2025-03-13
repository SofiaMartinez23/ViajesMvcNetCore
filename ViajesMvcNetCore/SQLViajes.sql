DROP TABLE IF EXISTS CHAT;
DROP TABLE IF EXISTS COMENTARIOS;
DROP TABLE IF EXISTS SEGUIDORES;
DROP TABLE IF EXISTS LUGARESFAVORITOS;
DROP TABLE IF EXISTS LUGARES;
DROP TABLE IF EXISTS USUARIOLOGIN;
DROP TABLE IF EXISTS LOG_IN;
DROP TABLE IF EXISTS USUARIOS;

-- Crear tabla USUARIOS
CREATE TABLE USUARIOS (
    ID_USUARIO INT PRIMARY KEY,
    NOMBRE NVARCHAR(255) NOT NULL,
    EMAIL NVARCHAR(255) UNIQUE NOT NULL,
    EDAD INT NOT NULL,
    NACIONALIDAD NVARCHAR(100) NOT NULL,
    PREFERENCIASDEVIAJE NVARCHAR(MAX) NOT NULL,
    IMAGEN NVARCHAR(255) NOT NULL,
    FECHADEREGISTRO DATE NOT NULL
);

-- Crear tabla LOGIN
CREATE TABLE LOG_IN (
    ID_USUARIO INT PRIMARY KEY,
    CORREO NVARCHAR(255) UNIQUE NOT NULL,
    CLAVE NVARCHAR(255) NOT NULL,
    FOREIGN KEY (ID_USUARIO) REFERENCES USUARIOS(ID_USUARIO)
);

-- Crear tabla USUARIOLOGIN
CREATE TABLE USUARIOLOGIN (
    ID_USUARIO INT PRIMARY KEY,
    CORREO NVARCHAR(255) UNIQUE NOT NULL,
	NOMBRE NVARCHAR(255) NOT NULL,
    CLAVE NVARCHAR(255) NOT NULL,
    CONFIRMARCLAVE NVARCHAR(255) NOT NULL,
    PREFERENCIASDEVIAJE NVARCHAR(MAX) NOT NULL,
	COLORAVATAR NVARCHAR(255) NOT NULL,
    AVATARURL NVARCHAR(255) NOT NULL,
    FOREIGN KEY (ID_USUARIO) REFERENCES USUARIOS(ID_USUARIO)
);

-- Crear tabla LUGARES
CREATE TABLE LUGARES (
    ID_LUGAR INT PRIMARY KEY,
    NOMBRE NVARCHAR(255) NOT NULL,
    DESCRIPCION NVARCHAR(MAX) NOT NULL,
    UBICACION NVARCHAR(255) NOT NULL,
    CATEGORIA NVARCHAR(255) NOT NULL,
    HORARIO DATETIME NOT NULL,
    IMAGEN NVARCHAR(255) NOT NULL,
    TIPO NVARCHAR(50) NOT NULL,
    ID_USUARIO INT NOT NULL,  -- Relacionamos cada lugar con un usuario
    CONSTRAINT FK_LUGARES_USUARIOS FOREIGN KEY (ID_USUARIO) REFERENCES USUARIOS(ID_USUARIO) -- Definimos la clave foránea
);
-- Crear tabla SEGUIDORES

CREATE TABLE SEGUIDORES (
    ID_SEGUIDOR INT PRIMARY KEY,
    ID_USUARIO_SEGUIDOR INT NOT NULL, -- El usuario que sigue
    ID_USUARIO_SEGUIDO INT NOT NULL, -- El usuario que es seguido
    FECHA_SEGUIMIENTO DATETIME NOT NULL,
    CONSTRAINT FK_SEGUIDORES_SEGUIDO FOREIGN KEY (ID_USUARIO_SEGUIDO) REFERENCES USUARIOS(ID_USUARIO)
);

-- Crear tabla LUGARESFAVORITOS
CREATE TABLE LUGARESFAVORITOS (
	ID_FAVORITO INT PRIMARY KEY,
    ID_USUARIO INT NOT NULL,
    ID_LUGAR INT NOT NULL,
    FECHADEVISITA_LUGAR DATE NOT NULL,
    IMAGEN_LUGAR NVARCHAR(255) NOT NULL,
    NOMBRE_LUGAR NVARCHAR(255) NOT NULL,
    DESCRIPCION_LUGAR NVARCHAR(MAX) NOT NULL,
    UBICACION_LUGAR NVARCHAR(255) NOT NULL,
    TIPO_LUGAR NVARCHAR(50) NOT NULL,
    FOREIGN KEY (ID_USUARIO) REFERENCES USUARIOS(ID_USUARIO),
    FOREIGN KEY (ID_LUGAR) REFERENCES LUGARES(ID_LUGAR)
);

-- Crear tabla COMENTARIOS
CREATE TABLE COMENTARIOS (
    ID_COMENTARIO INT PRIMARY KEY,
    ID_LUGAR INT NOT NULL,
    ID_USUARIO INT NOT NULL,
    COMENTARIO NVARCHAR(MAX) NOT NULL,
    FECHA_COMENTARIO DATETIME NOT NULL,
    FOREIGN KEY (ID_LUGAR) REFERENCES LUGARES(ID_LUGAR),
    FOREIGN KEY (ID_USUARIO) REFERENCES USUARIOS(ID_USUARIO)
);

-- Crear tabla CHAT
CREATE TABLE CHAT (
    ID_MENSAJE INT PRIMARY KEY,
    ID_USUARIO_REMITENTE INT NOT NULL,
    ID_USUARIO_DESTINATARIO INT NOT NULL,
    MENSAJE NVARCHAR(MAX) NOT NULL,
    FECHA_ENVIO DATETIME NOT NULL,
    FOREIGN KEY (ID_USUARIO_REMITENTE) REFERENCES USUARIOS(ID_USUARIO),
    FOREIGN KEY (ID_USUARIO_DESTINATARIO) REFERENCES USUARIOS(ID_USUARIO)
);

-- Insertar datos de ejemplo en USUARIOS
INSERT INTO USUARIOS (ID_USUARIO, NOMBRE, EMAIL, EDAD, NACIONALIDAD, PREFERENCIASDEVIAJE, IMAGEN, FECHADEREGISTRO)
VALUES
(1, N'Juan Pérez', N'juan.perez@email.com', 28, N'México', N'Aventura, Historia', N'https://img.freepik.com/foto-gratis/angulo-bosque-3d-arboles-ro0cas_23-2150800507.jpg', '2024-11-23'),
(2, N'María García', N'maria.garcia@email.com', 35, N'España', N'Cultura, Relax', N'https://img.freepik.com/foto-gratis/angulo-bosque-3d-arboles-ro0cas_23-2150800507.jpg', '2024-11-23'),
(3, N'Pedro Sánchez', N'pedro.sanchez@email.com', 22, N'Argentina', N'Playa, Relax', N'https://img.freepik.com/foto-gratis/angulo-bosque-3d-arboles-ro0cas_23-2150800507.jpg', '2025-02-06'),
(4, N'Luisa Fernández', N'luisa.fernandez@email.com', 40, N'Colombia', N'Naturaleza, Gastronomía', N'https://img.freepik.com/foto-gratis/angulo-bosque-3d-arboles-ro0cas_23-2150800507.jpg', '2025-01-15'),
(5, N'Carlos Rodríguez', N'carlos.rodriguez@email.com', 31, N'Chile', N'Montaña, Aventura', N'https://img.freepik.com/foto-gratis/angulo-bosque-3d-arboles-ro0cas_23-2150800507.jpg', '2024-12-20');

-- Insertar datos de ejemplo en LOG_IN, asegurando que ID_USUARIO coincida
INSERT INTO LOG_IN (ID_USUARIO, CORREO, CLAVE)
VALUES
(1, 'juan.perez@email.com', 'a'),
(2, 'maria.garcia@email.com', 'a1'),
(3, 'pedro.sanchez@email.com', 'a2'),
(4, 'luisa.fernandez@email.com', 'a3'),
(5, 'carlos.rodriguez@email.com', 'a4');

-- Insertar datos de ejemplo en USUARIOLOGIN, asegurando que ID_USUARIO y CORREO coincidan
INSERT INTO USUARIOLOGIN (ID_USUARIO, CORREO, NOMBRE, CLAVE, CONFIRMARCLAVE, PREFERENCIASDEVIAJE, ColorAvatar, AvatarUrl)
VALUES
(1, N'juan.perez@email.com', N'Juan Pérez', N'contrasena1', N'contrasena1', N'Aventura, Historia', N'#FF0000', N'https://img.freepik.com/foto-gratis/angulo-bosque-3d-arboles-ro0cas_23-2150800507.jpg'),
(2, N'maria.garcia@email.com', N'María García', N'contrasena2', N'contrasena2', N'Cultura, Relax', N'#00FF00', N'https://img.freepik.com/foto-gratis/angulo-bosque-3d-arboles-ro0cas_23-2150800507.jpg'),
(3, N'pedro.sanchez@email.com', N'Pedro Sánchez', N'contrasena3', N'contrasena3', N'Playa, Relax', N'#0000FF', N'https://img.freepik.com/foto-gratis/angulo-bosque-3d-arboles-ro0cas_23-2150800507.jpg'),
(4, N'luisa.fernandez@email.com', N'Luisa Fernández', N'contrasena4', N'contrasena4', N'Naturaleza, Gastronomía', N'#FFFF00', N'https://img.freepik.com/foto-gratis/angulo-bosque-3d-arboles-ro0cas_23-2150800507.jpg'),
(5, N'carlos.rodriguez@email.com', N'Carlos Rodríguez', N'contrasena5', N'contrasena5', N'Montaña, Aventura', N'#00FFFF', N'https://img.freepik.com/foto-gratis/angulo-bosque-3d-arboles-ro0cas_23-2150800507.jpg');

-- Insertar datos de ejemplo en LUGARES
INSERT INTO LUGARES (ID_LUGAR, NOMBRE, DESCRIPCION, UBICACION, CATEGORIA, HORARIO, IMAGEN, TIPO, ID_USUARIO)
VALUES
(1, 'Central Park', 'Un gran parque urbano con lagos, senderos y jardines.', 'Nueva York, EE.UU.', 'Parque', '2024-01-01T06:00:00', 'central_park.jpg', 'Publico',1),
(2, 'Museo del Louvre', 'Uno de los museos de arte más grandes y visitados del mundo.', 'París, Francia', 'Museo', '2024-01-01T09:00:00', 'louvre.jpg', 'Publico',1),
(3, 'Playa de Copacabana', 'Una famosa playa con arena blanca y un paseo marítimo animado.', 'Río de Janeiro, Brasil', 'Playa', '2024-01-01T00:00:00', 'copacabana.jpg', 'Publico',2),
(4, 'La Trattoria', 'Un restaurante italiano con auténtica cocina casera.', 'Roma, Italia', 'Restaurante', '2024-01-01T12:00:00', 'trattoria.jpg', 'Privado',3),
(5, 'La Gran Muralla China', 'Una serie de fortificaciones construidas a lo largo de las fronteras históricas del norte de China.', 'China', 'Monumento', '2024-01-01T08:00:00', 'gran_muralla.jpg', 'Publico',4);
-- Ejemplo 1: Un parque famoso

-- Insertar datos de ejemplo en LUGARESFAVORITOS
INSERT INTO LUGARESFAVORITOS (ID_FAVORITO,ID_USUARIO, ID_LUGAR, FECHADEVISITA_LUGAR, IMAGEN_LUGAR, NOMBRE_LUGAR, DESCRIPCION_LUGAR, UBICACION_LUGAR, TIPO_LUGAR)
VALUES 
(1, 1, 1, '2025-03-10', N'https://ejemplo.com/paris.jpg', N'París', N'Capital de Francia, famosa por su cultura.', N'Francia', N'Ciudad'),
(2, 1, 3, '2025-04-15', N'https://ejemplo.com/beijing.jpg', N'Beijing', N'Capital de China, famosa por su historia antigua.', N'China', N'Ciudad'),
(3, 1, 2, '2024-12-05', N'https://ejemplo.com/madrid.jpg', N'Madrid', N'Capital de España, conocida por su historia y cultura.', N'España', N'Ciudad'),
(4, 1, 4, '2025-05-20', N'https://ejemplo.com/roma.jpg', N'Roma', N'Capital de Italia, conocida por su historia antigua y monumentos.', N'Italia', N'Ciudad'),
(5, 1, 5, '2025-06-25', N'https://ejemplo.com/cancun.jpg', N'Cancún', N'Destino turístico famoso por sus playas de arena blanca y aguas cristalinas.', N'México', N'Playa');

INSERT INTO COMENTARIOS (ID_COMENTARIO, ID_LUGAR, ID_USUARIO, COMENTARIO, FECHA_COMENTARIO)
VALUES
(1, 1, 1, N'¡París es una ciudad increíble! Me encantó la Torre Eiffel.', GETDATE()),
(2, 1, 2, N'Totalmente de acuerdo. La comida y el ambiente son maravillosos.', GETDATE()),
(3, 1, 3, N'No puedo esperar a volver. Hay tanto que ver y hacer.', GETDATE()),
(4, 2, 1, N'Madrid tiene una vibra única. El Museo del Prado es imprescindible.', GETDATE()),
(5, 2, 2, N'Las tapas y la vida nocturna son geniales. ¡Definitivamente lo recomiendo!', GETDATE()),
(6, 2, 3, N'Me encantó caminar por el Parque del Retiro. Una ciudad hermosa.', GETDATE()),
(7, 5, 1, N'Cancún es el paraíso en la tierra. Las playas son espectaculares.', GETDATE()),
(8, 5, 2, N'Los resorts todo incluido son increíbles. Perfecto para relajarse.', GETDATE()),
(9, 5, 3, N'Me encantó bucear en los arrecifes. Una experiencia inolvidable.', GETDATE());
-- Insertar datos de ejemplo en CHAT
INSERT INTO CHAT (ID_MENSAJE, ID_USUARIO_REMITENTE, ID_USUARIO_DESTINATARIO, MENSAJE, FECHA_ENVIO)
VALUES
(7, 1, 5, N'Hola María, ¿cómo estás?', GETDATE()),
(1, 1, 2, N'Hola María, ¿cómo estás?', GETDATE()),
(2, 2, 1, N'Hola Juan, estoy bien gracias. ¿Y tú?', GETDATE()),
(3, 1, 3, N'Pedro, ¿has visitado algún lugar interesante últimamente?', GETDATE()),
(4, 3, 1, N'Hola Juan, sí, fui a la Gran Muralla hace poco. ¡Increíble!', GETDATE()),
(5, 4, 5, N'Carlos, ¿quieres ir a Cancún conmigo?', GETDATE()),
(6, 5, 4, N'¡Claro que sí, Luisa! Me encantaría.', GETDATE());

-- Insertar datos de ejemplo en SEGUIDORES
INSERT INTO SEGUIDORES (ID_SEGUIDOR, ID_USUARIO_SEGUIDOR, ID_USUARIO_SEGUIDO, FECHA_SEGUIMIENTO)
VALUES
(1, 1, 2, GETDATE()), 
(2, 1, 3, GETDATE()), 
(3, 2, 4, GETDATE()), 
(4, 3, 1, GETDATE()), 
(5, 4, 5, GETDATE());

select * from LUGARESFAVORITOS where ID_USUARIO=1

-- SP_INSERT_LUGARES (Modificado con CATEGORIA y HORARIO)
CREATE OR ALTER PROCEDURE SP_INSERT_LUGARES
    (
        @nombre NVARCHAR(255),
        @descripcion NVARCHAR(MAX),
        @ubicacion NVARCHAR(255),
        @categoria NVARCHAR(255),
        @horario DATETIME,
        @imagen NVARCHAR(255),
        @tipo NVARCHAR(50),
        @id_usuario INT  -- Nuevo parámetro para ID_USUARIO
    )
AS
BEGIN
    DECLARE @id_lugar INT;
    -- Generamos el nuevo ID_LUGAR automáticamente
    SELECT @id_lugar = ISNULL(MAX(ID_LUGAR), 0) + 1 FROM LUGARES;

    -- Insertamos el nuevo lugar
    INSERT INTO LUGARES (ID_LUGAR, NOMBRE, DESCRIPCION, UBICACION, CATEGORIA, HORARIO, IMAGEN, TIPO, ID_USUARIO)
    VALUES (@id_lugar, @nombre, @descripcion, @ubicacion, @categoria, @horario, @imagen, @tipo, @id_usuario);
END
GO


-- SP_GET_USUARIO_BY_EMAIL (Modificado)
CREATE OR ALTER PROCEDURE SP_GET_USUARIO_BY_EMAIL
    (@email NVARCHAR(255))
AS
BEGIN
    SELECT U.*, UL.CLAVE, UL.PREFERENCIASDEVIAJE AS PREFERENCIASDEVIAJE_LOGIN, UL.CORREO
    FROM USUARIOS U
    INNER JOIN USUARIOLOGIN UL ON U.ID_USUARIO = UL.ID_USUARIO
    WHERE U.EMAIL = @email;
END
GO

-- SP_GET_LUGARES_FAVORITOS (Modificado)
CREATE OR ALTER PROCEDURE SP_GET_LUGARES_FAVORITOS
    (@id_usuario INT)
AS
BEGIN
    SELECT L.*, U.NOMBRE AS NOMBRE_LIKE, U.IMAGEN AS IMAGEN_LIKE, U.ID_USUARIO AS ID_USUARIO_LIKE
    FROM LUGARES L
    INNER JOIN LUGARESFAVORITOS LF ON L.ID_LUGAR = LF.ID_LUGAR
    INNER JOIN USUARIOS U ON LF.ID_USUARIO = U.ID_USUARIO
    WHERE LF.ID_USUARIO = @id_usuario;
END
GO

-- SP_GET_LUGARES_CREADOS (Modificado)
CREATE OR ALTER PROCEDURE SP_GET_LUGARES_CREADOS
    (@id_usuario INT)
AS
BEGIN
    SELECT L.*
    FROM LUGARES L
    INNER JOIN LUGARESFAVORITOS LF ON L.ID_LUGAR = LF.ID_LUGAR
    WHERE LF.ID_USUARIO = @id_usuario;
END
GO

-- SP_GET_USUARIO_BY_ID (Modificado)
CREATE OR ALTER PROCEDURE SP_GET_USUARIO_BY_ID
    (@id_usuario INT)
AS
BEGIN
    SELECT U.*, UL.CLAVE, UL.PREFERENCIASDEVIAJE AS PREFERENCIASDEVIAJE_LOGIN, UL.CORREO
    FROM USUARIOS U
    INNER JOIN USUARIOLOGIN UL ON U.ID_USUARIO = UL.ID_USUARIO
    WHERE U.ID_USUARIO = @id_usuario;
END
GO

-- SP_INSERT_MENSAJE (Modificado)
CREATE OR ALTER PROCEDURE SP_INSERT_MENSAJE
    (@id_usuario_remitente INT, @id_usuario_destinatario INT, @mensaje NVARCHAR(MAX))
AS
BEGIN
    INSERT INTO CHAT (ID_USUARIO_REMITENTE, ID_USUARIO_DESTINATARIO, MENSAJE, FECHA_ENVIO)
    VALUES (@id_usuario_remitente, @id_usuario_destinatario, @mensaje, GETDATE());
END
GO

-- SP_GET_MENSAJES (Modificado)
CREATE OR ALTER PROCEDURE SP_GET_MENSAJES
    (@id_usuario_remitente INT, @id_usuario_destinatario INT)
AS
BEGIN
    SELECT *
    FROM CHAT
    WHERE (ID_USUARIO_REMITENTE = @id_usuario_remitente AND ID_USUARIO_DESTINATARIO = @id_usuario_destinatario)
    OR (ID_USUARIO_REMITENTE = @id_usuario_destinatario AND ID_USUARIO_DESTINATARIO = @id_usuario_remitente)
    ORDER BY FECHA_ENVIO;
END
GO

-- SP_GET_COMENTARIOS_LUGAR (Modificado)
CREATE OR ALTER PROCEDURE SP_GET_COMENTARIOS_LUGAR
    (@id_lugar INT)
AS
BEGIN
    SELECT C.*, U.NOMBRE AS NombreUsuario
    FROM COMENTARIOS C
    INNER JOIN USUARIOS U ON C.ID_USUARIO = U.ID_USUARIO
    WHERE C.ID_LUGAR = @id_lugar
    ORDER BY C.FECHA_COMENTARIO DESC;
END
GO

DECLARE @IdLugar INT;
SET @IdLugar = 1; -- Reemplaza 1 con el ID del lugar que deseas consultar

EXEC SP_GET_COMENTARIOS_LUGAR @id_lugar = @IdLugar;


-- SP_INSERT_COMENTARIO (Modificado)
CREATE OR ALTER PROCEDURE SP_INSERT_COMENTARIO
    (@id_lugar INT, @id_usuario INT, @comentario NVARCHAR(MAX))
AS
BEGIN
    INSERT INTO COMENTARIOS (ID_LUGAR, ID_USUARIO, COMENTARIO, FECHA_COMENTARIO)
    VALUES (@id_lugar, @id_usuario, @comentario, GETDATE());
END
GO

CREATE OR ALTER PROCEDURE SP_INSERT_USER
    (
        @nombre NVARCHAR(255),
        @correo NVARCHAR(255),
        @clave NVARCHAR(255),
        @confirmarclave NVARCHAR(255),
        @preferenciasdeviaje NVARCHAR(MAX),
        @coloravatar NVARCHAR(255),
        @avatarurl NVARCHAR(255)
    )
AS
BEGIN
    -- Obtener el último ID_USUARIO y sumarle 1
    DECLARE @id_usuario INT;
    SELECT @id_usuario = ISNULL(MAX(ID_USUARIO), 0) + 1 FROM USUARIOS;

    -- Insertar en la tabla USUARIOS
    INSERT INTO USUARIOS (ID_USUARIO, NOMBRE, EMAIL, EDAD, NACIONALIDAD, PREFERENCIASDEVIAJE, IMAGEN, FECHADEREGISTRO)
    VALUES (@id_usuario, @nombre, @correo, 0, 'Nacionalidad', @preferenciasdeviaje, '', GETDATE()); -- Valores por defecto para EDAD, NACIONALIDAD e IMAGEN

    -- Insertar en la tabla USUARIOLOGIN
    INSERT INTO USUARIOLOGIN (ID_USUARIO, CORREO, NOMBRE, CLAVE, CONFIRMARCLAVE, PREFERENCIASDEVIAJE, ColorAvatar, AvatarUrl)
    VALUES (@id_usuario, @correo, @nombre, @clave, @confirmarclave, @preferenciasdeviaje, @coloravatar, @avatarurl);

    -- Insertar en la tabla LOG_IN
    INSERT INTO LOG_IN (ID_USUARIO, CORREO, CLAVE)
    VALUES (@id_usuario, @correo, @clave);

END
GO

CREATE OR ALTER PROCEDURE SP_INSERT_COMENTARIO
    (
        @idLugar INT,
        @idUsuario INT,
        @comentario NVARCHAR(MAX)
    )
AS
BEGIN
    DECLARE @idcomentario INT;
    SELECT @idcomentario = ISNULL(MAX(ID_COMENTARIO), 0) + 1 FROM COMENTARIOS;

    INSERT INTO COMENTARIOS (ID_COMENTARIO, ID_LUGAR, ID_USUARIO, COMENTARIO, FECHA_COMENTARIO)
    VALUES (@idcomentario, @idLugar, @idUsuario, @comentario, GETDATE());
END
GO


SELECT
    U.ID_USUARIO,
    U.NOMBRE,
    U.EMAIL,
    U.EDAD,
    U.NACIONALIDAD,
    U.PREFERENCIASDEVIAJE,
    U.IMAGEN,
    U.FECHADEREGISTRO,
    L.CORREO AS CORREO_LOGIN,
    L.CLAVE
FROM
    USUARIOS U
JOIN
    LOG_IN L ON U.ID_USUARIO = L.ID_USUARIO
WHERE
    U.ID_USUARIO = 2;

	select * from LOG_IN




CREATE OR ALTER PROCEDURE SP_GET_LUGARES_POR_USUARIO
   ( @id_usuario INT)
AS
BEGIN
    -- Seleccionar los lugares implementados por un usuario
    SELECT L.ID_LUGAR, L.NOMBRE, L.DESCRIPCION, L.UBICACION, L.CATEGORIA, L.HORARIO, L.IMAGEN, L.TIPO
    FROM LUGARES L
    WHERE L.ID_USUARIO = @id_usuario;
END
GO



CREATE OR ALTER PROCEDURE SP_ALL_LUGARES
AS
BEGIN
    SELECT * FROM LUGARES
END
GO


CREATE OR ALTER PROCEDURE SP_UPDATE_LUGAR
    (
        @id_lugar INT,
        @nombre NVARCHAR(255),
        @descripcion NVARCHAR(MAX),
        @ubicacion NVARCHAR(255),
        @categoria NVARCHAR(255),
        @horario DATETIME,
        @imagen NVARCHAR(255),
        @tipo NVARCHAR(50)
    )
AS
BEGIN
    UPDATE LUGARES
    SET 
        NOMBRE = @nombre,
        DESCRIPCION = @descripcion,
        UBICACION = @ubicacion,
        CATEGORIA = @categoria,
        HORARIO = @horario,
        IMAGEN = @imagen,
        TIPO = @tipo
    WHERE ID_LUGAR = @id_lugar;
END
GO


CREATE OR ALTER PROCEDURE SP_DELETE_LUGAR
    (
        @idlugar INT
    )
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Eliminar registros relacionados en COMENTARIOS
        DELETE FROM COMENTARIOS
        WHERE ID_LUGAR = @idlugar;

        -- Eliminar registros relacionados en LUGARESFAVORITOS
        DELETE FROM LUGARESFAVORITOS
        WHERE ID_LUGAR = @idlugar;

        -- Eliminar el lugar de la tabla LUGARES
        DELETE FROM LUGARES
        WHERE ID_LUGAR = @idlugar;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Manejar el error (por ejemplo, registrarlo en una tabla de errores)
        THROW; -- Re-lanza el error para que la aplicación lo capture
    END CATCH;
END
GO



CREATE OR ALTER PROCEDURE SP_UPDATE_PERFIL
    @id_usuario INT,
    @nombre VARCHAR(255),
    @correo VARCHAR(255),
    @clave VARCHAR(255),
    @confirmarclave VARCHAR(255),
    @preferenciaviaje VARCHAR(255),
    @coloravatar VARCHAR(7),
    @avatarurl VARCHAR(255),
    @edad INT, -- Añadimos el parámetro para la edad
    @nacionalidad VARCHAR(100) -- Añadimos el parámetro para la nacionalidad
AS
BEGIN
    -- Actualizar USUARIOLOGIN
    UPDATE USUARIOLOGIN
    SET
        NOMBRE = @nombre,
        CORREO = @correo,
        CLAVE = @clave,
        CONFIRMARCLAVE = @confirmarclave,
        PREFERENCIASDEVIAJE = @preferenciaviaje,
        COLORAVATAR = @coloravatar,
        AVATARURL = @avatarurl
    WHERE ID_USUARIO = @id_usuario;

    -- Actualizar USUARIOS
    UPDATE USUARIOS
    SET
        EDAD = @edad,
        NACIONALIDAD = @nacionalidad
    WHERE ID_USUARIO = @id_usuario;
END;
GO


CREATE OR ALTER PROCEDURE SP_FAVORITOS_BY_USUARIO
    (@idusuario INT)
AS
BEGIN 
    SELECT  
       *
    FROM
        LUGARESFAVORITOS 
    WHERE  
        ID_USUARIO = @idusuario;
END
GO


EXEC SP_FAVORITOS_BY_USUARIO 1


CREATE OR ALTER PROCEDURE SP_AGREGAR_FAVORITO
    (@idusuario INT, @idlugar INT, @fecha DATE, @imagen NVARCHAR(255), 
     @nombre NVARCHAR(255), @descripcion NVARCHAR(MAX), @ubicacion NVARCHAR(255), @tipo NVARCHAR(50))
AS
BEGIN
    -- Declarar la variable para el siguiente ID_FAVORITO
    DECLARE @nuevoIDFavorito INT;

    -- Obtener el siguiente valor del ID_FAVORITO sumando 1 al valor máximo actual
    SELECT @nuevoIDFavorito = ISNULL(MAX(ID_FAVORITO), 0) + 1 FROM LUGARESFAVORITOS;

    -- Verificamos si el lugar ya está marcado como favorito por el usuario
    IF NOT EXISTS (SELECT 1 FROM LUGARESFAVORITOS WHERE ID_USUARIO = @idusuario AND ID_LUGAR = @idlugar)
    BEGIN
        -- Insertamos el nuevo favorito con el nuevo ID_FAVORITO
        INSERT INTO LUGARESFAVORITOS (ID_FAVORITO, ID_USUARIO, ID_LUGAR, FECHADEVISITA_LUGAR, IMAGEN_LUGAR, NOMBRE_LUGAR, 
                                      DESCRIPCION_LUGAR, UBICACION_LUGAR, TIPO_LUGAR)
        VALUES (@nuevoIDFavorito, @idusuario, @idlugar, @fecha, @imagen, @nombre, @descripcion, @ubicacion, @tipo);
    END
    ELSE
    BEGIN
        PRINT 'Este lugar ya está en tus favoritos.';
    END
END
GO





SELECT
    L.ID_LUGAR,
    L.NOMBRE,
    L.DESCRIPCION,
    L.UBICACION,
    L.CATEGORIA,
    L.HORARIO,
    L.IMAGEN,
    L.TIPO,
    LF.ID_FAVORITO
FROM
    LUGARESFAVORITOS LF
JOIN
    LUGARES L ON LF.ID_LUGAR = L.ID_LUGAR
WHERE
    LF.ID_USUARIO = 1;



CREATE OR ALTER PROCEDURE SP_DELETE_FAVORITO
    (@idusuario INT, @idlugar INT)
AS
BEGIN
    -- Eliminar el lugar de los favoritos del usuario
    DELETE FROM LUGARESFAVORITOS
    WHERE ID_USUARIO = @idusuario AND ID_LUGAR = @idlugar;

    -- Verificar si la eliminación fue exitosa
    IF @@ROWCOUNT = 0
    BEGIN
        PRINT 'No se encontró este lugar en los favoritos del usuario.';
    END
    ELSE
    BEGIN
        PRINT 'Lugar eliminado de los favoritos correctamente.';
    END
END
GO



CREATE OR ALTER PROCEDURE SP_DELETE_COMENTARIO
    (@idcomentario INT)
AS
BEGIN
    DELETE FROM COMENTARIOS
    WHERE ID_COMENTARIO = @idcomentario;
END;

EXEC SP_DELETE_COMENTARIO 11

DELETE FROM COMENTARIOS
    WHERE ID_COMENTARIO = 11;
select * from COMENTARIOS where ID_COMENTARIO = 14



CREATE OR ALTER PROCEDURE SP_UPDATE_COMENTARIO
    @id_comentario INT,
    @id_lugar INT,
    @id_usuario INT,
    @comentario NVARCHAR(MAX),
    @fecha_comentario DATETIME
AS
BEGIN
    UPDATE COMENTARIOS
    SET 
        ID_LUGAR = @id_lugar,
        ID_USUARIO = @id_usuario,
        COMENTARIO = @comentario,
        FECHA_COMENTARIO = @fecha_comentario
    WHERE ID_COMENTARIO = @id_comentario;
END

EXEC SP_UPDATE_COMENTARIO 14, 4, 1, 'holaaa', '2025-03-10 09:38:44.677'


CREATE PROCEDURE GetLugaresPorTipo
    @tipo NVARCHAR(50)
AS
BEGIN
    SELECT * 
    FROM LUGARES
    WHERE Tipo = @tipo;
END;

SELECT * 
    FROM LUGARES
    WHERE Tipo = 'Publico';



	CREATE OR ALTER VIEW VISTAUSUARIOCOMPLETO AS
SELECT
    U.ID_USUARIO,
    U.NOMBRE,
    U.EMAIL,
    U.EDAD,
    U.NACIONALIDAD,
    U.PREFERENCIASDEVIAJE,
    U.IMAGEN,
    U.FECHADEREGISTRO,
    UL.CORREO AS CORREOLOGIN,
    UL.CLAVE,
    UL.CONFIRMARCLAVE,
    UL.COLORAVATAR,
    UL.AVATARURL
FROM
    USUARIOS U
INNER JOIN
    USUARIOLOGIN UL ON U.ID_USUARIO = UL.ID_USUARIO;
GO



SELECT
    S.ID_USUARIO_SEGUIDO,
    U.NOMBRE AS NOMBRE_SEGUIDO,
    U.IMAGEN AS IMAGEN_SEGUIDO
FROM
    SEGUIDORES S
JOIN
    USUARIOS U ON S.ID_USUARIO_SEGUIDO = U.ID_USUARIO
WHERE
    S.ID_USUARIO_SEGUIDOR = 1;



CREATE OR ALTER VIEW VISTA_USUARIOS_SEGUIDOS_PERFIL AS
SELECT DISTINCT
    S.ID_SEGUIDOR,
    S.ID_USUARIO_SEGUIDOR,
    S.ID_USUARIO_SEGUIDO,
    U.NOMBRE AS NOMBRE_SEGUIDO,
    U.IMAGEN AS IMAGEN_SEGUIDO,
    S.FECHA_SEGUIMIENTO
FROM
    SEGUIDORES S
JOIN
    USUARIOS U ON S.ID_USUARIO_SEGUIDO = U.ID_USUARIO;
GO


select * from VISTA_USUARIOS_SEGUIDOS_PERFIL 

CREATE OR ALTER PROCEDURE SP_SEGUIDORES_BY_USUARIO
    @idusuario INT
AS
BEGIN
    -- Seleccionar los usuarios seguidos desde la vista, filtrando por ID_USUARIO_SEGUIDOR
    SELECT
        ID_SEGUIDOR,
        ID_USUARIO_SEGUIDOR,
        ID_USUARIO_SEGUIDO,
        NOMBRE_SEGUIDO,
        IMAGEN_SEGUIDO,
        FECHA_SEGUIMIENTO
    FROM
        VISTA_USUARIOS_SEGUIDOS_PERFIL
    WHERE
        ID_USUARIO_SEGUIDOR = @idusuario;
END
GO


EXEC SP_SEGUIDORES_BY_USUARIO 6

select * from LUGARESFAVORITOS where ID_USUARIO=1
select * from COMENTARIOS

CREATE OR ALTER PROCEDURE SP_ADD_SEGUIR
    @idusuarioseguidor INT,
    @idusuarioseguido INT,
    @fechaseguimiento DATETIME
AS
BEGIN
    DECLARE @ID_SEGUIDOR INT;

    -- Obtener el valor actual máximo de ID_SEGUIDOR y aumentarlo en 1
    SELECT @ID_SEGUIDOR = ISNULL(MAX(ID_SEGUIDOR), 0) + 1 FROM SEGUIDORES;

    -- Insertar el registro de seguimiento con el nuevo ID_SEGUIDOR
    INSERT INTO SEGUIDORES (ID_SEGUIDOR, ID_USUARIO_SEGUIDOR, ID_USUARIO_SEGUIDO, FECHA_SEGUIMIENTO)
    VALUES (@ID_SEGUIDOR, @idusuarioseguidor, @idusuarioseguido, @fechaseguimiento);

    -- Retornar el ID del seguidor insertado (opcional, si se necesita este valor)
    SELECT @ID_SEGUIDOR AS ID_SEGUIDOR;
END;

CREATE OR ALTER PROCEDURE SP_DELETE_SEGUIDOR
    (@idusuarioseguidor INT, @idusuarioseguido INT)
AS
BEGIN
    DELETE FROM SEGUIDORES
    WHERE ID_USUARIO_SEGUIDOR = @idusuarioseguidor 
      AND ID_USUARIO_SEGUIDO = @idusuarioseguido;
END;
EXEC SP_DELETE_SEGUIDOR @idusuarioseguidor = 1, @idusuarioseguido = 3;


select * from VISTA_USUARIOS_SEGUIDOS_PERFIL where ID_USUARIO_SEGUIDOR = 6
select * from LUGARESFAVORITOS where ID_USUARIO = 1
