exports.up = async function (knex) {
    await knex.raw(`
        IF EXISTS (SELECT * FROM sys.foreign_keys WHERE referenced_object_id = object_id('category'))
        BEGIN
            DECLARE @sql NVARCHAR(MAX) = '';
            SELECT @sql += 'ALTER TABLE ' + OBJECT_SCHEMA_NAME(parent_object_id) + '.' + OBJECT_NAME(parent_object_id) + 
                          ' DROP CONSTRAINT ' + name + ';'
            FROM sys.foreign_keys
            WHERE referenced_object_id = object_id('category');
            EXEC sp_executesql @sql;
        END;
        DROP TABLE IF EXISTS category;
        CREATE TABLE category(  
            id int NOT NULL,
            name nvarchar(50) not null
        );
    `);
};

exports.down = async function (knex) {
    await knex.raw(`
        IF EXISTS (SELECT * FROM sys.foreign_keys WHERE referenced_object_id = object_id('category'))
        BEGIN
            DECLARE @sql NVARCHAR(MAX) = '';
            SELECT @sql += 'ALTER TABLE ' + OBJECT_SCHEMA_NAME(parent_object_id) + '.' + OBJECT_NAME(parent_object_id) + 
                          ' DROP CONSTRAINT ' + name + ';'
            FROM sys.foreign_keys
            WHERE referenced_object_id = object_id('category');
            EXEC sp_executesql @sql;
        END;
        DROP TABLE IF EXISTS category;
        CREATE TABLE category(  
            id int IDENTITY(0,1) primary key,
            name nvarchar(50) not null
        );
    `);
};
