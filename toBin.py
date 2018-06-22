import sys
sanat = sys.argv[1].split('.')
numerot = []
for w in sanat:
    num = int(w)
    binary = format(num, 'b')
    while len(binary) < 8:
        binary = '0' + binary
    numerot.append(binary)

jointer = ' '
result = jointer.join(numerot)
print(result)
