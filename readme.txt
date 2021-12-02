190201019-190201132
Yusuf Eymen Sezen-Abd Alkadir Hasani


Projemizi çalıştırmak için Visual Studio çalışma ortamını açınız.
Ardından MS Sql Server veritabanında "sudoku" adında bir veritabanı oluşturunuz.
CellSolutions tablosunu:

CREATE TABLE [dbo].[CellSolutions] (
    [Id]        INT      IDENTITY (1, 1) NOT NULL,
    [X]         INT      NOT NULL,
    [Y]         INT      NOT NULL,
    [Date]      DATETIME NOT NULL,
    [N]         INT      NOT NULL,
    [IsThread5] BIT      NOT NULL,
    [ThreadId]  INT      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

şeklinde oluşturduktan sonra projeyi çalıştırabilirsiniz.
