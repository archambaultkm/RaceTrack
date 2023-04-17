# RaceTrack

A procedurally generated mesh creates a new track for each game. Right now the road will always draw linearly along the z-axis, with random values along a certain range for the x-axis. 

This mesh works pretty well for straight bits of road, but sometimes in more jagged areas mesh triangles will draw on the wrong side of the  axis resulting in holes.

Early iterations of the mesh had much worse success drawing a road without holes, caused by incorrectly calculating the edge of the road to the left of the generated path
![Screen Shot 2023-04-16 at 4 46 31 PM](https://user-images.githubusercontent.com/97715354/232568251-374f227d-50c9-4320-a729-0f6093025e4d.png)

Road generated with no relationship between points' x-axis location:
![Screen Shot 2023-04-16 at 6 18 20 PM](https://user-images.githubusercontent.com/97715354/232568432-94530ec3-4be5-49d4-8579-b32281e4e02f.png)

Examples of generated roads using the current mesh (You can see that some attempts are still more successful than others:

![Screen Shot 2023-04-17 at 2 34 23 PM](https://user-images.githubusercontent.com/97715354/232567264-3fbf025b-2cd9-4281-b3cf-18e14f686e2c.png)
![Screen Shot 2023-04-17 at 2 35 58 PM](https://user-images.githubusercontent.com/97715354/232567266-018e056b-e49b-4f0f-8f48-98f67c7bd5a5.png)
![Screen Shot 2023-04-17 at 2 34 56 PM](https://user-images.githubusercontent.com/97715354/232567267-2dc777fd-929b-4358-9cf1-fc25400d3f62.png)

Player View:

![Screen Shot 2023-04-17 at 2 36 45 PM](https://user-images.githubusercontent.com/97715354/232567327-94d8856a-dabc-4e30-93c8-3d0412c02c7b.png)
