# EcoSys Simulation

**EcoSys Simulation** est une application permettant de simuler un écosystème où des plantes, herbivores, et carnivores interagissent dans un environnement dynamique. Chaque entité possède des comportements uniques tels que la reproduction, la consommation d'énergie et les interactions avec d'autres entités.
![image](https://github.com/user-attachments/assets/e7971f20-0005-48df-ab3a-953ca736d5e1)

## Table des matières

- [Aperçu du projet](#aperçu-du-projet)
- [Fonctionnalités](#fonctionnalités)
- [Structure du code](#structure-du-code)
- [Prérequis](#prérequis)
- [Installation](#installation)
- [Utilisation](#utilisation)

---

## Aperçu du projet

Ce projet repose sur une architecture basée sur **MVVM (Model-View-ViewModel)**, conçue pour une application WPF. Les interactions entre les entités de l'écosystème suivent des règles biologiques simples, comme :

- Les herbivores consomment des plantes pour survivre.
- Les carnivores chassent les herbivores pour se nourrir.
- Les entités peuvent se reproduire selon des conditions spécifiques.

L'objectif est de modéliser un cycle de vie complet de l'écosystème en temps réel.

---

## Fonctionnalités

- Simulation en temps réel des interactions entre les plantes, herbivores et carnivores.
- Gestion de la reproduction (avec une période de gestation pour les carnivores et les herbivores).
- Calcul dynamique de l'énergie consommée par le déplacement, la chasse ou la reproduction.
- Mise à jour visuelle de l'état de l'écosystème dans une interface utilisateur.

---

## Structure du code

### Classes principales

- **`EcosysObjet` :** Classe de base pour toutes les entités.
- **`Plante` :** Représente les plantes de l'écosystème.
- **`Herbivore` :** Les animaux herbivores, capables de manger des plantes et de fuir les carnivores.
- **`Carnivore` :** Les animaux carnivores, chasseurs d'herbivores.
- **`Gestation<T>` :** Classe générique gérant la reproduction des herbivores et carnivores.
- **`MainWindowViewModel` :** Contrôle la logique principale de la simulation et gère l'interface utilisateur.

### Points clés

- L'application utilise le **modèle MVVM** pour séparer la logique métier (ViewModel) de la présentation (View).
- Le comportement des entités est encapsulé dans leurs classes respectives, avec des interactions via des événements déclenchés dans `MainWindowViewModel`.

---

## Prérequis

- .NET 6.0 ou version ultérieure.
- Visual Studio 2022 (ou tout autre IDE compatible avec WPF).
- Bibliothèques spécifiques (listées dans le fichier `.csproj`).

---

## Installation

1. Clonez le dépôt :

   ```bash
   git clone https://github.com/ChaimaeAjn/Ecosys.git
   cd Ecosys
   ```

2. Ouvrez le projet dans **Visual Studio**.

3. Compilez et exécutez l'application.

---

## Utilisation

1. Lancez l'application.
2. Configurez les paramètres initiaux de l'écosystème via l'interface utilisateur (nombre d'entités, taille de la carte, etc.).
3. Appuyez sur **Démarrer** pour lancer la simulation.
4. Observez les interactions et les mises à jour en temps réel.

---



