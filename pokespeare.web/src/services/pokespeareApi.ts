import axios from 'axios';
import { IPokemonListResponse, IPokemonResponse } from '../models';

const baseURL = process.env.REACT_APP_BASE_URL ? process.env.REACT_APP_BASE_URL : 'http://localhost:5070/api/v1/';

const take = 20;

const pokespeareApi = axios.create({
    baseURL,
});

export const listPokemon  = async (skip = 0): Promise<IPokemonListResponse> => {
    var response = await pokespeareApi.get<IPokemonListResponse>(`/pokemon?take=${take}&skip=${skip}`);
    return response.data;
};

export const searchPokemon  = async (name: string, skip = 0): Promise<IPokemonListResponse> => {
    var response = await pokespeareApi.get<IPokemonListResponse>(`/pokemon?searchTerm=${name}&take=${take}&skip=${skip}`);
    return response.data;
};

export const getPokemon = async (id: number) : Promise<IPokemonResponse> => {
    var response = await pokespeareApi.get<IPokemonResponse>(`/pokemon/${id}`);
    return response.data;
};