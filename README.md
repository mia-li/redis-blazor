# redis-blazor

install redis in docker 

docker run --name my-redis -p 5002:6379 -d redis

docker exec -it my-redis sh //enter the container

#redis-cli

scan 0

hgetall key

