CREATE TABLE [dbo].[OEDependencies] (
    [OEId]         BIGINT NOT NULL,
    [DependencyId] BIGINT NOT NULL,
    CONSTRAINT [PK_OEDependencies] PRIMARY KEY CLUSTERED ([OEId] ASC, [DependencyId] ASC),
    CONSTRAINT [FK_OEDependencies_Dependencies] FOREIGN KEY ([DependencyId]) REFERENCES [dbo].[Dependencies] ([DependencyId]),
    CONSTRAINT [FK_OEDependencies_OEs] FOREIGN KEY ([OEId]) REFERENCES [dbo].[OEs] ([OEId])
);




GO
CREATE TRIGGER dbo.TR_OEDependenciesInsUpdDel
   ON dbo.OEDependencies 
   AFTER INSERT, DELETE, UPDATE

AS 

BEGIN

	SET NOCOUNT ON;

	UPDATE Dependencies
	SET EffectiveITAR = dbo.DependencyIsITAR(D.DependencyId)
	FROM Dependencies D
		INNER JOIN
		(SELECT DependencyId FROM inserted
			UNION
		 SELECT DependencyId FROM deleted) X ON X.DependencyId = D.DependencyId
END