!  Console1.f90 
!
!  FUNCTIONS:
!  Console1 - Entry point of console application.
!

!****************************************************************************
!
!  PROGRAM: Console1
!
!  PURPOSE:  Entry point for the console application.
!
!****************************************************************************

    program Console1

    implicit none

         character (len = 15) :: first_name
         
             integer i,sum
     integer, parameter :: np=100000000
      do i=1,np
         sum = sum
      enddo
    ! Body of Console1
      print *, 'Hello World'
      
   print *,' Enter your first name.' 
   print *,' Up to 20 characters, please'
   
   read *,first_name 
   print "(1x,a)",first_name
    end program Console1

