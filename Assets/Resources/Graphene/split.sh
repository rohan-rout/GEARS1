#!/bin/bash

for((i = 0; i < 201; i++)); do
	start=$(echo $i | awk '{printf("%d", $1*4538)}')
	end=$(echo $i | awk '{printf("%d", ($1+1)*4538)}')

	fileName=$(echo $i | awk '{printf("%09d", $1)}')
	echo "4536" >> ${fileName}.xyz
	echo "200	200	200	90	90	90" >> ${fileName}.xyz
	head -n $end graphene.xyz | tail -n 4538 >> ${fileName}.xyz
	echo $fileName
done
