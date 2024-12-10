# флаг --rm удалит контейнер после остановки.
# после запуска контейнера откройте в браузере http://localhost:9090/swagger

# image from dockerhub
docker run --rm -d --name kutumov-t-4 -p 9090:8080 noncommunicado/unistream-test-4:latest
# ИЛИ
# local builded image
docker run --rm -d --name kutumov-t-4 -p 9090:8080 unistream-test-4:latest