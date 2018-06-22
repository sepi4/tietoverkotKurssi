import sys
# sanat = sys.argv
# numerot = []
# for w in sanat[1:]:
#     num = int(w, 2)
#     numerot.append(str(num))

# jointer = '.'
# result = jointer.join(numerot)
# print(result)


sanat = sys.argv
sanat = [str(int(s,2)) for s in sanat[1:]]

jointer = '.'
result = jointer.join(sanat)
print(result)