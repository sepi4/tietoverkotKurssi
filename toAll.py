import sys

arr = sys.argv[1].split('/')
numerot = arr[0].split('.')
montako = int(arr[1])

# print(numerot)
print(montako)

vast = []
for w in numerot:
    num = int(w)
    binary = format(num, 'b')
    while len(binary) < 8:
        binary = '0' + binary
    vast.append(binary)

jointer = '.'
result = jointer.join(vast)
print(vast)
