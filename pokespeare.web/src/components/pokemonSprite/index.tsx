import React  from 'react';

import { IPokemonItem } from '../../models/index';
import { Container } from './styles';

interface PokemonSpriteProps {
    selectedPokemon: IPokemonItem;
}

const pokemonSprite: React.FC<PokemonSpriteProps> = ({ selectedPokemon }) => {
    return (
        <Container src={selectedPokemon.sprite} />
    );
};

export default pokemonSprite;  