SET client_encoding = 'UTF8';

INSERT INTO "Clients" ("Id", "AccountNumber", "FullName", "Phone", "Email", "Notes")
VALUES
    (1, 'LS-0001', 'Иванов Сергей Петрович', '+7 (8452) 44-10-20', 'ivanov@example.ru', 'Частный дом, договор ТО действует до конца года'),
    (2, 'LS-0002', 'Петрова Анна Викторовна', '+7 (8452) 55-23-11', 'petrova@example.ru', 'Квартира, требуется согласование времени визита'),
    (3, 'LS-0003', 'Смирнов Алексей Игоревич', '+7 (8452) 62-77-08', 'smirnov@example.ru', 'Коммерческий объект'),
    (4, 'LS-0004', 'Кузнецов Дмитрий Андреевич', '+7 (927) 150-33-44', 'kuznetsov@example.ru', 'Повторная заявка после аварийного отключения');

INSERT INTO "ClientAddresses" ("Id", "ClientId", "City", "Street", "House", "Apartment", "Entrance", "Floor", "FullAddress")
VALUES
    (1, 1, 'Саратов', 'Московская', '12', '', '', '', 'г. Саратов, ул. Московская, д. 12'),
    (2, 2, 'Саратов', 'Тархова', '31', '48', '2', '6', 'г. Саратов, ул. Тархова, д. 31, кв. 48'),
    (3, 3, 'Энгельс', 'Строителей', '8', '', '', '1', 'г. Энгельс, пр-т Строителей, д. 8'),
    (4, 4, 'Саратов', 'Чернышевского', '90', '15', '1', '3', 'г. Саратов, ул. Чернышевского, д. 90, кв. 15');

INSERT INTO "Masters" ("Id", "FullName", "Phone", "Specialization", "Qualification", "IsActive")
VALUES
    (1, 'Соколов Андрей Николаевич', '+7 (927) 200-10-01', 'Ремонт газовых котлов', '5 разряд', true),
    (2, 'Морозов Павел Сергеевич', '+7 (927) 200-10-02', 'Плановое техническое обслуживание', '4 разряд', true),
    (3, 'Орлова Марина Викторовна', '+7 (927) 200-10-03', 'Проверка безопасности и автоматики', 'Инженер', true),
    (4, 'Егоров Илья Романович', '+7 (927) 200-10-04', 'Аварийные работы', '5 разряд', false);

INSERT INTO "Equipment" ("Id", "ClientId", "AddressId", "SerialNumber", "Type", "InstallationDate", "NextInspectionDate", "Manufacturer", "Model", "Location", "Status")
VALUES
    (1, 1, 1, 'BAXI-24F-2021-001', 'Газовый котел', '2021-09-15 10:00:00', '2026-09-15 10:00:00', 'BAXI', 'ECO Four 24 F', 'Котельная, линия 1', 'В эксплуатации'),
    (2, 2, 2, 'VAIL-TEC-2020-014', 'Газовый котел', '2020-11-03 14:30:00', '2026-06-01 09:00:00', 'Vaillant', 'turboTEC plus VUW', 'Кухня', 'Требует обслуживания'),
    (3, 3, 3, 'BUD-LOG-2022-103', 'Регулятор давления газа', '2022-04-22 09:15:00', '2026-05-25 09:00:00', 'Buderus', 'Logamax U072', 'ГРП, шкаф 2', 'В эксплуатации'),
    (4, 4, 4, 'ARIS-CLAS-2019-088', 'Газовый водонагреватель', '2019-06-10 12:00:00', '2026-05-20 09:00:00', 'Ariston', 'Clas X 24 FF', 'Ванная комната', 'Остановлено'),
    (5, 1, 1, 'NAV-DELUXE-2023-045', 'Газовый котел', '2023-02-18 11:20:00', '2027-02-18 11:20:00', 'Navien', 'Deluxe S 24K', 'Котельная, линия 2', 'В эксплуатации');

INSERT INTO "ServiceRequests" ("Id", "ClientId", "AddressId", "EquipmentId", "MasterId", "RequestDate", "DeadlineDate", "CompletionDate", "RequestType", "Priority", "Status", "Description")
VALUES
    (1, 1, 1, 1, 2, '2026-05-01 09:00:00', '2026-05-03 18:00:00', '2026-05-01 16:30:00', 'Плановое обслуживание', 'Обычный', 'Завершена', 'Плановое техническое обслуживание, проверка автоматики и тяги.'),
    (2, 2, 2, 2, 1, '2026-05-06 11:15:00', '2026-05-08 18:00:00', NULL, 'Ремонт', 'Высокий', 'В работе', 'Нестабильное давление, требуется диагностика горелки.'),
    (3, 3, 3, 3, 3, '2026-05-09 08:45:00', '2026-05-10 18:00:00', '2026-05-10 13:10:00', 'Проверка безопасности', 'Обычный', 'Завершена', 'Замена фильтра и контроль герметичности соединений.'),
    (4, 4, 4, 4, NULL, '2026-05-12 15:20:00', '2026-05-13 18:00:00', NULL, 'Аварийная заявка', 'Срочный', 'Открыта', 'Оборудование отключено после аварийного срабатывания датчика.'),
    (5, 1, 1, 5, 2, '2026-05-18 10:30:00', '2026-05-25 18:00:00', NULL, 'Плановый осмотр', 'Обычный', 'Открыта', 'Подготовка к сезонному техническому осмотру.');

INSERT INTO "WorkRecords" ("Id", "ServiceRequestId", "MasterId", "WorkDate", "Cost", "WorkType", "MaterialsUsed", "Result")
VALUES
    (1, 1, 2, '2026-05-01 15:40:00', 3500.00, 'Техническое обслуживание котла', 'Прокладки, чистящее средство', 'Оборудование исправно, тяга в норме.'),
    (2, 3, 3, '2026-05-10 12:30:00', 2800.00, 'Проверка герметичности', 'Фильтр газовый', 'Утечек не обнаружено, фильтр заменен.');

INSERT INTO "InspectionResults" ("Id", "ServiceRequestId", "EquipmentId", "InspectionDate", "IsSafe", "GasLeakCheck", "VentilationCheck", "AutomationCheck", "Conclusion", "Recommendations")
VALUES
    (1, 1, 1, '2026-05-01 16:00:00', true, 'Утечек нет', 'Тяга соответствует норме', 'Автоматика исправна', 'Эксплуатация разрешена', 'Следующее ТО по графику.'),
    (2, 3, 3, '2026-05-10 13:00:00', true, 'Утечек нет', 'Вентиляция исправна', 'Датчики срабатывают корректно', 'Эксплуатация разрешена', 'Повторная проверка через 12 месяцев.');

SELECT setval(pg_get_serial_sequence('"Clients"', 'Id'), COALESCE((SELECT MAX("Id") FROM "Clients"), 1));
SELECT setval(pg_get_serial_sequence('"ClientAddresses"', 'Id'), COALESCE((SELECT MAX("Id") FROM "ClientAddresses"), 1));
SELECT setval(pg_get_serial_sequence('"Masters"', 'Id'), COALESCE((SELECT MAX("Id") FROM "Masters"), 1));
SELECT setval(pg_get_serial_sequence('"Equipment"', 'Id'), COALESCE((SELECT MAX("Id") FROM "Equipment"), 1));
SELECT setval(pg_get_serial_sequence('"ServiceRequests"', 'Id'), COALESCE((SELECT MAX("Id") FROM "ServiceRequests"), 1));
SELECT setval(pg_get_serial_sequence('"WorkRecords"', 'Id'), COALESCE((SELECT MAX("Id") FROM "WorkRecords"), 1));
SELECT setval(pg_get_serial_sequence('"InspectionResults"', 'Id'), COALESCE((SELECT MAX("Id") FROM "InspectionResults"), 1));
