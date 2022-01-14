import React  from 'react';

import { IPokemonItem } from '../../models/index';
import { Container } from './styles';
import { Card } from '../../styles/global';

interface PokemonNameProps {
    selectedPokemon: IPokemonItem;
}

const pokemonName: React.FC<PokemonNameProps> = ({ selectedPokemon }) => {
    return (
        <Card backgroundColor={selectedPokemon.type}>
            <Container>
                <h1 data-testid="pokemonName">{selectedPokemon.name}</h1>
            </Container>
        </Card>
    );
};

export default pokemonName;  