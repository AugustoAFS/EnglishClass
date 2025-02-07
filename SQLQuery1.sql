CREATE DATABASE englishclass;
use englishclass;

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- ID auto-incrementado de 1 em 1 e chave prim�ria 
    Username NVARCHAR(50) NOT NULL,  -- Nome de usu�rio obrigat�rio
    Email NVARCHAR(100) NOT NULL,  -- Email obrigat�rio
    PasswordHash NVARCHAR(255) NOT NULL,  -- Senha criptografada, obrigat�ria
    CreatedAt DATETIME DEFAULT GETDATE()  -- Data de cria��o com valor padr�o = data atual
);


CREATE TABLE Languages (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE UserLanguages (
    UserId INT NOT NULL,
    LanguageId INT NOT NULL,
    PRIMARY KEY (UserId, LanguageId),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (LanguageId) REFERENCES Languages(Id) ON DELETE CASCADE
);

CREATE TABLE Words (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    LanguageId INT NOT NULL,
    Word NVARCHAR(100) NOT NULL,
    Translation NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (LanguageId) REFERENCES Languages(Id) ON DELETE CASCADE
);
