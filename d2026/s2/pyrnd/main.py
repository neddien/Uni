#!/usr/bin/env python3

def randstuff():
    import random

    # պատահական float [0, 1) միջակայքում
    print(random.random())

    # պատահական float [1.5, 10.5] միջակայքում
    print(random.uniform(1.5, 10.5))

    # պատահական թիվ [a, b] միջակայքում
    print(random.randint(1, 100))

    # պատահական էլեմենտ հաջորդականությունից
    colors = ["red", "green", "blue", "yellow"]
    print(random.choice(colors))

    # խառնել ցուցակը տեղում
    random.shuffle(colors)
    print(colors)

def splitstuff():
    # կարդալ տողը, բաժանել ըստ բացատների, կանչել reverse
    stdin = input()
    tokens = stdin.split()
    tokens.reverse()
    result = " ".join(tokens)
    print(result)

randstuff()
splitstuff()
