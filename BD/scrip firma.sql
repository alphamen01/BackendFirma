CREATE TABLE firmaDigital (
id_firma SERIAL UNIQUE NOT NULL PRIMARY KEY,
tipo_firma CHARACTER (1) NOT NULL,
razon_social VARCHAR (200) NOT NULL,
representante_legal VARCHAR (200) NOT NULL,
empresa_acreditadora VARCHAR (200) NOT NULL,
fecha_emision TIMESTAMP NOT NULL,
fecha_vencimiento TIMESTAMP NOT NULL,
ruta_rubrica TEXT NOT NULL,
certificado_digital TEXT NOT NULL
)

INSERT INTO firmaDigital (tipo_firma, razon_social, representante_legal, empresa_acreditadora, fecha_emision, fecha_vencimiento, ruta_rubrica, certificado_digital)
VALUES
    ('T', 'Empresa A', 'Juan Pérez', 'Acreditadora X', '2023-08-22 10:00:00', '2023-12-31 23:59:59', '/ruta/rubrica1.jpg', 'certificado1'),
    ('F', 'Empresa B', 'Ana Gómez', 'Acreditadora Y', '2023-07-15 09:30:00', '2023-11-30 23:59:59', '/ruta/rubrica2.jpg', 'certificado2'),
    ('E', 'Empresa C', 'Luis Rodríguez', 'Acreditadora Z', '2023-09-10 14:15:00', '2023-10-31 23:59:59', '/ruta/rubrica3.jpg', 'certificado3');
	
SELECT * FROM firmadigital

CREATE TABLE firmaDigital (
id_firma SERIAL UNIQUE NOT NULL IDENTITY PRIMARY KEY,
tipo_firma CHARACTER (1) NOT NULL,
razon_social VARCHAR (200) NOT NULL,
representante_legal VARCHAR (200) NOT NULL,
empresa_acreditadora VARCHAR (200) NOT NULL,
fecha_emision TIMESTAMP NOT NULL,
fecha_vencimiento TIMESTAMP NOT NULL,
ruta_rubrica TEXT NOT NULL,
certificado_digital TEXT NOT NULL
)