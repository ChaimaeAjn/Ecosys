#Une description d’au moins deux principes SOLID utilisés dans le projet.

## 1. Le principe de responsabilité unique (SRP) avec le code Animal.cs

Le principe de responsabilité unique (SRP) stipule qu'une classe ou une méthode doit être responsable d'une seule chose. Cela permet de garder un code plus cohérent et facile à maintenir.

Dans notre code, la classe **Animal** respecte ce principe.

### Voici pourquoi :

- **Gestion de l'énergie et de la santé** :  
  La classe **Animal** gère des propriétés comme **Energy**, **Hearts**, et **IsDead** pour représenter l'état de santé et l'énergie de l'animal.

- **Méthode LoseHeart()** :  
  Elle gère la perte de cœurs de l'animal et la mort lorsqu'il n'a plus de cœurs. Cette logique est encapsulée dans cette méthode, ce qui garantit que la gestion de la santé et de l'énergie reste concentrée dans une seule partie du code.

- **Mouvement abstrait avec Move()** :  
  La classe **Animal** ne gère pas directement le déplacement, elle crée une méthode abstraite **Move()**, ce qui permet à chaque sous-classe de définir comment l'animal se déplace. Cela sépare la gestion de l'énergie et de la santé de la gestion du mouvement, rendant le code plus propre et plus organisé.

Ainsi, chaque aspect de l'animal (sa santé, son mouvement, son énergie) est bien séparé, ce qui simplifie les modifications futures et l'ajout de nouvelles fonctionnalités sans perturber les autres parties du code.

---

## 2. Le principe de l'ouverture/fermeture (OCP) avec le code de Carnivore.cs

Le principe d'ouverture/fermeture (OCP) nous dit qu'une classe doit être ouverte à l'extension (on peut y ajouter de nouvelles fonctionnalités) mais fermée à la modification (on ne doit pas modifier son code existant). Cela rend le système plus flexible et moins sujet aux erreurs lors de l'ajout de nouvelles fonctionnalités.

Dans notre code, la classe **Carnivore** respecte ce principe grâce à l'héritage.

### Voici comment :

- **Extension sans modification de la classe de base** :  
  La classe **Carnivore** hérite de **Animal** et ajoute des comportements spécifiques, comme la méthode **Move()** qui définit le déplacement propre à un carnivore. Elle peut utiliser les propriétés de **Animal** comme **Energy**, **Hearts**, et **IsDead** sans avoir à modifier la classe **Animal**.

- **Ajout de nouveaux types d'animaux sans toucher à Animal** :  
  Si l'on souhaite ajouter un nouvel animal, comme un **Herbivore** ou un **Omnivore**, il suffit de créer une nouvelle classe qui hérite de **Animal**. On n'a pas besoin de toucher ni à la classe **Carnivore**, ni à **Animal**. Cela permet d'ajouter de nouveaux types d'animaux sans perturber le système existant.

- **Ajout de nouvelles fonctionnalités dans Carnivore** :  
  Par exemple, on a ajouté la méthode **EatHerbivore()** pour simuler un carnivore mangeant un herbivore. Cette fonctionnalité a été ajoutée directement dans la classe **Carnivore** sans avoir à modifier la classe **Animal**. Chaque sous-classe peut ainsi ajouter des comportements spécifiques sans affecter le reste du code.
  
# Le diagramme d'activités

![image](https://github.com/user-attachments/assets/9049111e-eabe-4726-b2d2-dc73fb07e5bf)

# Le diagramme de séquence

# Le diagramme de classes

