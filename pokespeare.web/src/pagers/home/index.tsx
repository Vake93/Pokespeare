import React, { useEffect, useState } from 'react';
import { Col, Row } from 'react-grid-system';
import InfiniteScroll from 'react-infinite-scroll-component';

import SearchInput from '../../components/searchInput';
import pokespeareApi from '../../services/pokespeareApi';
import { IPokemonTypeName } from '../../util/colours';

import {
    Card,
    Container,
    PokemonList,
    PokemonName,
    PokemonSprite,
    Details,
    Item
} from './styles';

interface IPokemonListResponse {
    totalCount: number;
    data: IPokemonListItem[];
}

interface IPokemonResponse {
    data: IPokemonItem;
}

interface IPokemonListItem {
    id: number;
    name: string;
}

interface IPokemonItem {
    id: number;
    name: string;
    description: string;
    sprite: string;
    type: IPokemonTypeName;
    translated: boolean;
}

const Home: React.FC = () => {
    const [loading, setLoading] = useState(true);

    const [pokemonList, setPokemonList] = useState<IPokemonListResponse>({
        totalCount: 0,
        data: []
    } as IPokemonListResponse);

    const [pokemon, setPokemon] = useState<IPokemonItem>({} as IPokemonItem);

    const [search, setSearch] = useState('');

    const buildPokemonListUrl = (reload: boolean) => {
        const take = 20;
        const skip = reload ? 0 : pokemonList.data.length;

        return (search === '') ?
            `/pokemon?take=${take}&skip=${skip}` :
            `/pokemon?searchTerm=${search}&take=${take}&skip=${skip}`;
    };

    useEffect(() => {
        const url = buildPokemonListUrl(true);
        pokespeareApi.get<IPokemonListResponse>(url).then(response => {
            const pokemon = response.data;

            if (pokemon && pokemon.data.length > 0) {
                setPokemonList(pokemon);

                const firstPokemonData = pokemon.data[0];
                updatePokemon(firstPokemonData.id);
            }
        });
    }, []);

    useEffect(() => {
        const url = buildPokemonListUrl(true);
        pokespeareApi.get<IPokemonListResponse>(url).then(response => {
            const pokemon = response.data;

            if (pokemon && pokemon.data.length > 0) {
                setPokemonList(pokemon);

                const firstPokemonData = pokemon.data[0];
                updatePokemon(firstPokemonData.id);
            }
        });
    }, [search]);

    const searchPokemon = async (searchTerm: string) => {
        setSearch(searchTerm);
    };

    const loadMorePokemon = async () => {
        const url = buildPokemonListUrl(false);
        pokespeareApi.get<IPokemonListResponse>(url).then(response => {
            setPokemonList({
                totalCount: response.data.totalCount,
                data: [...pokemonList.data, ...response.data.data]
            });
        });
    };

    const updatePokemon = async (id: number) => {
        if (pokemon.id !== id) {
            pokespeareApi.get<IPokemonResponse>(`/pokemon/${id}`).then(response => {
                const pokemonResponse = response.data;
                setPokemon(pokemonResponse.data);
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
                        handleSearch={searchPokemon}
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
                                            onClick={() => updatePokemon(p.id)}
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