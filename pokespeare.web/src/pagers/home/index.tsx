import React, { useEffect, useState } from 'react';
import { Col, Row } from 'react-grid-system';

import SearchInput from '../../components/searchInput';
import PokemonName from '../../components/pokemonName';
import PokemonList from '../../components/pokemonList';
import PokemonSprite from '../../components/pokemonSprite';
import PokemonDescription from '../../components/pokemonDescription';

import { listPokemon, searchPokemon, getPokemon } from '../../services/pokespeareApi';
import { IPokemonListResponse, IPokemonItem } from '../../models/index';

import { Container } from './styles';

const Home: React.FC = () => {
    const [loading, setLoading] = useState(true);

    const [pokemonList, setPokemonList] = useState<IPokemonListResponse>({
        totalCount: 0,
        data: []
    } as IPokemonListResponse);

    const [pokemon, setPokemon] = useState<IPokemonItem>({} as IPokemonItem);

    const [search, setSearch] = useState('');

    useEffect(() => {
        listPokemon().then(pokemon => {
            if (pokemon && pokemon.data.length > 0) {
                setPokemonList(pokemon);

                const firstPokemonData = pokemon.data[0];
                selectPokemon(firstPokemonData.id);
            }
        });
    }, []);

    useEffect(() => {
        searchPokemon(search).then(pokemon => {
            if (pokemon && pokemon.data.length > 0) {
                setPokemonList(pokemon);

                const firstPokemonData = pokemon.data[0];
                selectPokemon(firstPokemonData.id);
            }
        });
    }, [search]);

    const loadMorePokemon = async () => {
        listPokemon(pokemonList.data.length).then(pokemon => {
            setPokemonList({
                totalCount: pokemon.totalCount,
                data: [...pokemonList.data, ...pokemon.data]
            });
        });
    };

    const selectPokemon = async (id: number) => {
        if (pokemon.id !== id) {
            getPokemon(id).then(pokemon => {
                setPokemon(pokemon.data);
                setLoading(false);
            });
        }
    };

    if (loading) return <div />;

    return (
        <Container>
            <Row>
                <Col lg={5}>
                    <SearchInput
                        type="text"
                        placeholder="Search"
                        handleSearch={setSearch}
                    />
                    <PokemonList
                        selectedPokemon={pokemon}
                        pokemonList={pokemonList}
                        loadMorePokemon={loadMorePokemon}
                        selectPokemon={selectPokemon}
                    />
                </Col>

                <Col lg={7}>
                    <Row>
                        <Col>
                            <PokemonName selectedPokemon={pokemon} />
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <PokemonSprite selectedPokemon={pokemon} />
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <PokemonDescription selectedPokemon={pokemon} />
                        </Col>
                    </Row>
                </Col>
            </Row>
        </Container>
    );
};

export default Home;