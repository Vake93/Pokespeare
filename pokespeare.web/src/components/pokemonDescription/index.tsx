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
                <p data-testid='description'>{selectedPokemon.description}</p>
                <p data-testid='translated'>{`Shakespearean: ${selectedPokemon.translated}`}</p>
            </div>
        </Container>
    );
};

export default pokemonDescription;  