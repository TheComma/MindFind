CREATE TABLE [dbo].[Photos] (
    [Photo_id]  INT            IDENTITY (1, 1) NOT NULL,
    [Tag_id]    INT            NULL,
    [ImagePath] NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_Photo] PRIMARY KEY CLUSTERED ([Photo_id] ASC),
    CONSTRAINT [FK_Photos_Tags] FOREIGN KEY ([Tag_id]) REFERENCES [dbo].[Tags] ([Tag_id]) ON UPDATE CASCADE
);







