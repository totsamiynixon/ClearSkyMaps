# ClearSkyMaps
This project is used for building safe routes without toxic and dangerous areas!

# Usefull links

1. https://developers.google.com/maps/documentation/javascript/examples/circle-simple
2. https://developers.google.com/maps/documentation/javascript/geometry#isLocationOnEdge
3. https://developers.google.com/maps/documentation/javascript/examples/poly-containsLocation

#TO DO

1. Создать CalculationService который будет определять уровень угрозы по показаниям, по сенсору, за период и тд
2. Возвращать из сервиса уровень опасност
3. Добавить на фронт переключатель допустимого уровня опасности
4. Отправлять на фронт вместе с показателями уровень последние рассчеты по уровню опасности и привязывать их к сенсору
5. Подбирать сенсоры для проверки в isOnEdge на основе выбранного уровня опасности