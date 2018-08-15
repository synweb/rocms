/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Если нет ресурсов
IF (SELECT COUNT(*) FROM [dbo].[CmsResource]) = 0
BEGIN
    SET IDENTITY_INSERT [dbo].[CmsResource] ON
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (1, N'Users', N'Пользователи')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (2, N'News', N'Новости')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (3, N'Gallery', N'Галерея')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (4, N'Albums', N'Альбомы')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (5, N'CommonSettings', N'Настройка')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (6, N'Menus', N'Меню')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (7, N'Pages', N'Страницы')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (8, N'Blocks', N'Блоки')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (9, N'Sliders', N'Слайдеры')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (10, N'Shop', N'Магазин')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (11, N'Reviews', N'Отзывы')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (13, N'Analytics', N'Аналитика')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (15, N'AdminPanel', N'Админка')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (16, N'Shop_Actions', N'Магазин: Акции')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (17, N'Shop_Orders', N'Магазин: Заказы')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (18, N'Shop_Packs', N'Магазин: Упаковки')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (19, N'Shop_Clients', N'Магазин: Клиенты')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (20, N'Shop_Reviews', N'Магазин: Отзывы')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (21, N'Shop_YmlExport', N'Магазин: Экспорт YML')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (22, N'Shop_Settings', N'Магазин: Настройки')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (23, N'Shop_PickUpPoints', N'Магазин: Пункты самовывоза')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (24, N'Shop_RegularClients', N'Магазин: Постоянным клиентам')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (25, N'DeleteObjects', N'Удаление объектов')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (26, N'SupportTickets', N'Поддержка')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (27, N'Shop_Currencies', N'Магазин: Валюты')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (28, N'Shop_OrderForms', N'Магазин: Формы заказа')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (100, N'Shop_MassPriceChange', N'Магазин: Пакетные изменения цен')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (34, N'CommentsEditor', N'Комментарии')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (40, N'Emails', N'Письма')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (50, N'UploadFiles', N'Файлы')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (51, N'Requests', N'База заявок')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (52, N'FAQ', N'Вопросы')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (60, N'Development', N'Разработка')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (61, N'Dev_CodeEditor', N'Разработка: Редактор кода')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (62, N'Dev_Widgets', N'Разработка: Виджеты')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (63, N'Dev_InterfaceStrings', N'Разработка: Строки интерфейса')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (64, N'Dev_Database', N'Разработка: База данных')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (65, N'RedirectToPageRoutes', N'Редиректы')
    INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (70, N'Dev_MagicButton', N'Разработка: Кнопка входа')

    SET IDENTITY_INSERT [dbo].[CmsResource] OFF
    
END
GO


-- Если нет юзверя admin
IF (SELECT COUNT(*) FROM [dbo].[User] WHERE [Username]='admin') = 0
BEGIN
    SET IDENTITY_INSERT [dbo].[User] ON
    -- Login/Password: admin/admin
    INSERT INTO [dbo].[User] ([UserId], [Username], [Password]) VALUES (1, N'admin', N'21232f297a57a5a743894a0e4a801fc3')
    SET IDENTITY_INSERT [dbo].[User] OFF

    -- Выдача всех прав админу
    INSERT INTO [dbo].[UserCmsResource]
    SELECT [CmsResourceId], 1
    FROM [dbo].[CmsResource]
END
GO

-- Если нет страниц
--IF (SELECT COUNT(*) FROM [dbo].[Page]) = 0
--BEGIN
--	SET IDENTITY_INSERT [dbo].[Page] ON
--	INSERT INTO [dbo].[Page] ([Title], [Annotation], [Content], [CreationDate], [RelativeUrl], [Keywords], [PageId]) VALUES (N'Главная', N'Главная страница', N'<p>Главная страница</p>
--	', GETUTCDATE(), N'Главная', NULL, 1)
--	SET IDENTITY_INSERT [dbo].[Page] OFF
--END
--GO

-- Если нет блоков
IF (SELECT COUNT(*) FROM [dbo].[Block]) = 0
BEGIN
    SET IDENTITY_INSERT [dbo].[Block] ON
    INSERT INTO [dbo].[Block] ([BlockId], [Title], [Content]) VALUES (1, N'Шапка', N'<p>Шапка</p>')
    INSERT INTO [dbo].[Block] ([BlockId], [Title], [Content]) VALUES (2, N'Подвал', N'<p>Подвал</p>')
    INSERT INTO [dbo].[Block] ([BlockId], [Title], [Content]) VALUES (3, N'Метрики', N'<p></p>')
    INSERT INTO [dbo].[Block] ([BlockId], [Title], [Content]) VALUES (4, N'Копирайт', N'<p>&copy;</p>')
    SET IDENTITY_INSERT [dbo].[Block] OFF
END
GO

-- Если нет меню
IF (SELECT COUNT(*) FROM [dbo].Menu) = 0
BEGIN
    SET IDENTITY_INSERT [dbo].[Menu] ON
    INSERT INTO [dbo].[Menu] ([MenuId], [Name]) VALUES (1, N'Главное')
    SET IDENTITY_INSERT [dbo].[Menu] OFF
END
GO

-- Если нет элементов меню
--IF (SELECT COUNT(*) FROM [dbo].MenuItem) = 0
--BEGIN
--	SET IDENTITY_INSERT [dbo].[MenuItem] ON
--	INSERT INTO [dbo].[MenuItem] ([MenuItemId], [Name], [MenuId], [ParentMenuItemId], [PageUrl], [SortOrder]) VALUES (1, N'Главная', 1, NULL, N'Главная', 0)
--	SET IDENTITY_INSERT [dbo].[MenuItem] OFF
--END
--GO

-- Если нет настроек
IF (SELECT COUNT(*) FROM [dbo].Setting) = 0
BEGIN
    --SET IDENTITY_INSERT [dbo].[Setting] ON
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'MainMenuId', N'1')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'MainPageUrl', N'Главная')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'SiteName', N'Новый сайт')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'Reviews', N'False')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'TicketLifetime', N'02:00:00')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'CommentsPremoderation', N'False')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'Timezone', 3)
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'MailTmplOrderAutoReply', N'<p>Здравствуйте, {0}!<br /><br />Мы получили Ваше обращение и свяжемся с Вами.<br /><br />Данные, которые Вы нам отправили:<br /><br />{1}</p>')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'MailTmplOrder', N'<p><strong>Имя клиента: </strong>{0}</p>
<p><strong>Email: </strong>
<a href="mailto:{1}">{1}
</a>
</p>
<p><strong>Телефон: </strong>{2}</p>
<p>{4}</p>')
        INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'MailTmplCallback', N'<h1>Обратный звонок</h1>
<p><strong>Имя клиента: </strong>{0}</p>
<p><strong>Телефон: </strong>{1}</p>
<p><strong>Удобное время: </strong>{2}</p>')
        INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'MailTmplReviewCreated', N'<h1>Новый отзыв</h1>
<p><strong>Имя: </strong>{0}</p>
<p><strong>Email: </strong>{1}</p>
<p><strong>Текст отзыва:</strong></p>
<p>{2}</p>')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'RootUrl', N'http://localhost:4014')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'ImageMaxHeight', 1080)
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'ImageMaxWidth', 1920)
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'ImageQuality', 90)
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'ThumbnailHeight', 350)
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'ThumbnailWidth', 300)
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'AutoEmailReplyEnabled', N'True')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'EmailSmtpUrl', N'smtp.yandex.ru')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'EmailSmtpPort', 587)
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'SmtpSslEnabled', N'True')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'EmailLogin', N'mail1@yandex.ru')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'EmailPassword', N'mailPassword')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'OrderEmailAddress', N'mail2@yandex.ru')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'SystemEmailAddress', N'mail1@yandex.ru')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'SystemEmailSenderName', N'Автоматическое сообщение')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'TranslitEnabled', N'True')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'AllowedFileExtensions', N'.doc,.docx,.xls,.xlsx,.pdf,.txt')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'YoutubeAPIKey', N'AIzaSyCQgKGPuIQwKvZvFLHjZ_sjr3ZB8ijQ4rA')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'ThumbnailSizes', N'400w,200h')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'ReviewCreatedNotification', N'False')
    INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'ReviewSort', N'CreationDateDesc')
    --SET IDENTITY_INSERT [dbo].[Setting] OFF
END
GO

-- Если нет стран
IF (SELECT COUNT(*) FROM [dbo].Country) = 0
BEGIN
    INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (1, N'Россия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (2, N'Австралия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (3, N'Австрия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (4, N'Азербайджан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (5, N'Албания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (6, N'Алжир')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (7, N'Ангилья (Ангуилла)')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (8, N'Ангола')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (9, N'Андорра')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (10, N'Антигуа и Барбуда')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (11, N'Аргентина')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (12, N'Армения')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (13, N'Аруба')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (14, N'Афганистан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (15, N'Багамские острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (16, N'Бангладеш')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (17, N'Барбадос')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (18, N'Бахрейн')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (19, N'Беларусь')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (20, N'Белиз')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (21, N'Бельгия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (22, N'Бенин')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (23, N'Бермудские острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (24, N'Болгария')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (25, N'Боливия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (26, N'Босния и Герцеговина')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (27, N'Ботсвана')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (28, N'Бразилия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (29, N'Британские Виргинские острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (30, N'Бруней')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (31, N'Буркина-Фасо')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (32, N'Бурунди')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (33, N'Бутан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (34, N'Вануату')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (35, N'Ватикан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (36, N'Великобритания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (37, N'Венгрия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (38, N'Венесуэла')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (39, N'Виргинские острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (40, N'Восточное (Американское) Самоа')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (41, N'Восточный Тимор')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (42, N'Вьетнам')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (43, N'Габон')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (44, N'Гаити')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (45, N'Гайана')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (46, N'Гамбия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (47, N'Гана')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (48, N'Гваделупа')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (49, N'Гватемала')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (50, N'Гвинея')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (51, N'Гвинея-Бисау')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (52, N'Германия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (53, N'Гибралтар')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (54, N'Гондурас')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (55, N'Гренада')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (56, N'Гренландия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (57, N'Греция')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (58, N'Грузия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (59, N'Гуам')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (60, N'Дания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (61, N'Демократическая Республика Конго')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (62, N'Джибути')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (63, N'Доминика')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (64, N'Доминиканская республика')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (65, N'Египет')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (66, N'Замбия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (67, N'Западная Сахара')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (68, N'Западное Самоа')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (69, N'Зимбабве')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (70, N'Израиль')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (71, N'Индия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (72, N'Индонезия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (73, N'Иордания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (74, N'Ирак')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (75, N'Иран')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (76, N'Ирландия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (77, N'Исландия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (78, N'Испания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (79, N'Италия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (80, N'Йемен')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (81, N'Кабо-Верде')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (82, N'Казахстан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (83, N'Каймановы острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (84, N'Камбоджа')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (85, N'Камерун')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (86, N'Канада')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (87, N'Катар')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (88, N'Кения')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (89, N'Кипр')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (90, N'Киргизия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (91, N'Кирибати')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (92, N'Китай')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (93, N'КНДР')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (94, N'Кокосовые острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (95, N'Колумбия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (96, N'Коморские острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (97, N'Коста-Рика')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (98, N'Кот-д`Ивуар')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (99, N'Куба')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (100, N'Кувейт')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (101, N'Кука острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (102, N'Лаос')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (103, N'Латвия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (104, N'Лесото')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (105, N'Либерия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (106, N'Ливан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (107, N'Ливия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (108, N'Литва')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (109, N'Лихтенштейн')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (110, N'Люксембург')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (111, N'Маврикий')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (112, N'Мавритания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (113, N'Мадагаскар')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (114, N'Макао (Аомынь)')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (115, N'Македония')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (116, N'Малави')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (117, N'Малайзия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (118, N'Мали')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (119, N'Мальдивы')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (120, N'Мальта')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (121, N'Марокко')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (122, N'Мартиника')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (123, N'Маршаловы острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (124, N'Мексика')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (125, N'Мидуэй')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (126, N'Микронезия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (127, N'Мозамбик')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (128, N'Молдавия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (129, N'Монако')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (130, N'Монголия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (131, N'Монтсеррат')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (132, N'Мьянма ( Бирма )')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (133, N'Намибия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (134, N'Народная Республика Конго')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (135, N'Науру')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (136, N'Непал')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (137, N'Нигер')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (138, N'Нигерия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (139, N'Нидерландские Антиллы')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (140, N'Нидерланды')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (141, N'Никарагуа')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (142, N'Ниуэ')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (143, N'Новая Зеландия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (144, N'Новая Каледония')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (145, N'Норвегия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (146, N'Норфолк')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (147, N'ОАЭ')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (148, N'Оман')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (149, N'Пакистан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (150, N'Палау')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (151, N'Палестина')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (152, N'Панама')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (153, N'Папуа-Новая Гвинея')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (154, N'Парагвай')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (155, N'Перу')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (156, N'Питкэрн')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (157, N'Польша')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (158, N'Португалия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (159, N'Пуэрто-Рико')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (160, N'Реюньон')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (161, N'Рождества остров')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (162, N'Руанда')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (163, N'Румыния')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (164, N'Сальвадор')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (165, N'Сан-Марино')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (166, N'Сан-Томе и Принсипи')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (167, N'Саудовская Аравия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (168, N'Свазиленд')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (169, N'Святой Елены Остров')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (170, N'Северные Марианские острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (171, N'Сейшельские острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (172, N'Сенегал')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (173, N'Сен-Пьер и Микелон')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (174, N'Сент-Винсент и Гренадины')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (175, N'Сент-Китс и Невис')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (176, N'Сент-Люсия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (177, N'Сербия и Черногория')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (178, N'Сингапур')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (179, N'Сирия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (180, N'Словакия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (181, N'Словения')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (182, N'Сомали')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (183, N'Судан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (184, N'Суринам')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (185, N'США')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (186, N'Сьерра-Леоне')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (187, N'Таджикистан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (188, N'Таиланд')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (189, N'Танзания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (190, N'Тёркс и Кайкос')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (191, N'Того')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (192, N'Токелау')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (193, N'Тонга')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (194, N'Тринидад и Тобаго')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (195, N'Тувалу')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (196, N'Тунис')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (197, N'Туркменистан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (198, N'Турция')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (199, N'Уганда')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (200, N'Узбекистан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (201, N'Украина')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (202, N'Уоллис и Футуна')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (203, N'Уругвай')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (204, N'Уэйк')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (205, N'Фарерские острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (206, N'Фиджи')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (207, N'Филиппины')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (208, N'Финляндия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (209, N'Фолклендские (Мальвинские) острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (210, N'Франция')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (211, N'Французская полинезия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (212, N'Хорватия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (213, N'Центрально-Африканская республика')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (214, N'ЧАД')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (215, N'Черногория')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (216, N'Чехия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (217, N'Чили')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (218, N'Швейцария')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (219, N'Швеция')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (220, N'Шри-Ланка')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (221, N'Эквадор')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (222, N'Экваториальная Гвинея')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (223, N'Эритрея')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (224, N'Эстония')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (225, N'Эфиопия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (226, N'Южная Корея')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (227, N'Южно-Африканская Республика')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (228, N'Ямайка')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (229, N'Япония')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (230, N'«Гвиана» Франция')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (231, N'«Майотта» Франция')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (232, N'«Парасельские острова» КНР, Вьетнам, Тайвань')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (233, N'«Сеута и Мелилья» Испания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (234, N'«Спартли острова» КНР, Вьетнам, Тайвань, Малайзия, Филиппины, Бруней')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (235, N'«Чагос острова» Великобритания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (236, N'«Шпицберген» Норвегия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (237, N'«Южная Георгия» Великобритания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (238, N'«Южные Сандвичевы острова» Фолклендские острова')
END
GO
