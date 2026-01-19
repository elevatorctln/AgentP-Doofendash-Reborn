#!/bin/bash
for i in {0..25}; do
    mkdir -p "Obstacle$i"
    mv Idle_$i.* "Obstacle$i/" 2>/dev/null
done
echo "Done!"
