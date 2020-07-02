# Ecliptic
Клиентская часть геоинформационной системы зданий
  Разрабатываемый программный продукт является мобильным приложением и носит название «Клиентская часть геоинформационной системы зданий». 
Назначение разработки данной выпускной квалификационной работы – создание Android приложения, обеспечивающего просмотр графических схем этажей здания, 
загруженного с серверной подсистемы, информации о его работниках и помещениях, а также построения маршрутов между локациями. 
  Целью разработки программного продукта является обеспечение возможности комфортной ориентации пользователя в незнакомом для него здании 
  посредством использования мобильного приложения, а также получения информации о интересующих его помещениях.
 Для достижения поставленной цели необходимо решить ряд задач:
-	обеспечение возможности загрузки на устройство любого доступного на сервере здания;
-	обеспечение возможности ознакомления пользователя с подробным планом этажей загруженного здания;
-	обеспечение возможности просмотра информации о любом помещении здания и его ответственных лицах;
-	упрощение задачи навигации внутри здания путем автоматизированного составления кратчайших маршрутов между выбранными пользователем помещениями;
-	обеспечение возможности создания и авторизованного доступа в личный аккаунт пользователя;
-	предоставление авторизированному пользователю возможности создания персональных и публичных заметок, а также добавления помещений в список избранных и удаления из него.
  С помощью разрабатываемого программного продукта будет упрощен процесс поиска необходимых помещений и путей к ним, просмотра информации о схемах этажей, а также получения информации о помещениях и их персонале. Например, на данный момент в Курском Государственном Университете студенты, особенно первокурсники, часто сталкиваются с необходимостью поиска нужной аудитории, в таком случае есть несколько вариантов действия: найти человека, знающего расположение всех аудиторий и который не против проводить вас до него или самостоятельно перемещаться по всем этажам здания, ориентируясь только по схемам эвакуации. Разумеется, представление варианты не являются комфортными по временным затратам, особенно при регулярном использовании. 
  
  Программный продукт разработан на основе платформы Xamarin Forms. 
  Загрузка информации о здании, его структуре и помещениях, а также обновление данных личного кабинета происходит путем отправки соответствующих HTTP запросов на внешние API серверной подсистемы и получения запрашиваемых данных в формате JSON. 
Для этого, естественно, необходимо подключение к серверной подсистеме через мобильную сеть. 
После загрузки с сервера, данные сохраняются на устройстве в локальной СУБД, что позволяет в дальнейшем использовать приложение и в оффлайн режиме. 
Для хранения данных используется компактная встраиваемая СУБД –  SQLite, повсеместно используемая на мобильных устройствах. С целью упрощения процесса разработки и поддержки базы была использована объектно-ориентированная технология для доступа к данным Entity Framework Core. Позволившая абстрагироваться от СУБД и работать с данными, как если бы мы работали с обычными объектами.

