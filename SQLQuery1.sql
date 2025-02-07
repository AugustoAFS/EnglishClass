CREATE DATABASE englishclass;
use englishclass;

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- ID auto-incrementado de 1 em 1 e chave primária 
    Username NVARCHAR(50) NOT NULL,  -- Nome de usuário obrigatório
    Email NVARCHAR(100) NOT NULL,  -- Email obrigatório
    PasswordHash NVARCHAR(255) NOT NULL,  -- Senha criptografada, obrigatória
    CreatedAt DATETIME DEFAULT GETDATE()  -- Data de criação com valor padrão = data atual
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
