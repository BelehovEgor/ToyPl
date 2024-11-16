# toy pl

## Как запускать
+ передать в аргументы 
	+ путь до исполняемого файла
	+ (опционально) величину n (default 5) 
+ ввести переменные, которая попросила программа из файла

## Особенности реализации
+ При переполнении целочисленного типа выбирается значение по модулю.

## Примеры
_tests/ToyPl.UnitTests/ToyPlExamples_

+ volume.tpl - вычисление объема параллелепипеда **in** (a b c ~~s~~), **out** (~~a b c~~ s)
+ fast-times.tpl - вычисление произведения 2 чисел **in** (a b ~~z~~), **out** (~~a b~~ z)
+ mults.tpl - вывод всех множителей числа **in** (a ~~b c~~), **out** (~~a b~~ c)
+ fibbonachi.tpl - вычисление чисел фиббоначи **in** (~~a b~~ n), **out** (a ~~b n~~)
+ euclidean.tpl - алгоритм эвклида вычисления НОД **in** (a b), **out** (a ~~b~~)

+ boom.tpl - ломаем время исполнения недетерминизмом программы
  + n = 3 - 1ms
  + n = 4 - 112ms
  + n = 5 - 340ms
  + n = 6 - 26s 391ms
  + n = 7 - 4m 34s 
