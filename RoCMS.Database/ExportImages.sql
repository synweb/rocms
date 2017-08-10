-- Выполнять после выкачивания картинок до изменения таблицы Image
UPDATE [Image] SET [ImageId] = [ImageId] + '.jpg' WHERE [MimeType] = 'image/jpeg'
UPDATE [Image] SET [ImageId] = [ImageId] + '.png' WHERE [MimeType] = 'image/png'
UPDATE [Image] SET [ImageId] = [ImageId] + '.gif' WHERE [MimeType] = 'image/gif'
