import React, { useEffect, useState } from 'react';
import { Col, Row } from 'react-grid-system';
import InfiniteScroll from 'react-infinite-scroll-component';

import SearchInput from '../../components/searchInput';
import { listPokemon, searchPokemon, getPokemon } from '../../services/pokespeareApi';
import { IPokemonListResponse, IPokemonItem } from '../../models/index';

import {
    Card,
    Container,
    PokemonList,
    PokemonName,
    PokemonSprite,
    Details,
    Item
} from './styles';

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
                    <PokemonList backgroundColor={pokemon.type}>
                        <div id="scrollableDiv">
                            <InfiniteScroll
                                dataLength={pokemonList.data.length}
                                next={loadMorePokemon}
                                hasMore={pokemonList.data.length < pokemonList.totalCount}
                                loader={<h4>Loading...</h4>}
                                endMessage={<p>That's all folks!</p>}
                                scrollableTarget="scrollableDiv">
                                {
                                    pokemonList.data.map(p => (
                                        <Item
                                            key={p.id}
                                            onClick={() => selectPokemon(p.id)}
                                            selected={pokemon.id === p.id}
                                        >
                                            <p>{`${p.id} - ${p.name}`}</p>
                                        </Item>
                                    ))
                                }
                            </InfiniteScroll>
                        </div>
                    </PokemonList>
                </Col>

                <Col lg={7}>
                    <Row>
                        <Col>
                            <Card backgroundColor={pokemon.type}>
                                <PokemonName>
                                    <h1>{pokemon.name}</h1>
                                </PokemonName>
                            </Card>
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <PokemonSprite src={pokemon.sprite} />
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <Details backgroundColor={pokemon.type}>
                                <div>
                                    <p>{pokemon.description}</p>
                                    <p>{`Shakespearean: ${pokemon.translated}`}</p>
                                </div>
                            </Details>
                        </Col>
                    </Row>
                </Col>
            </Row>
        </Container>
    );
};

export default Home;