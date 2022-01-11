import React from 'react';

import { IPokemonItem } from '../../models/index';
import { Container } from './styles';

interface PokemonDescriptionProps {
    selectedPokemon: IPokemonItem;
}

const pokemonDescription: React.FC<PokemonDescriptionProps> = ({ selectedPokemon }) => {
    return (
        <Container backgroundColor={selectedPokemon.type}>
            <div>
                <p>{selectedPokemon.description}</p>
                <p>{`Shakespearean: ${selectedPokemon.translated}`}</p>
            </div>
        </Container>
    );
};

export default pokemonDescription;  