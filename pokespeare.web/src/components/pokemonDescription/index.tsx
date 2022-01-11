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
                <p role='description'>{selectedPokemon.description}</p>
                <p role='translated'>{`Shakespearean: ${selectedPokemon.translated}`}</p>
            </div>
        </Container>
    );
};

export default pokemonDescription;  