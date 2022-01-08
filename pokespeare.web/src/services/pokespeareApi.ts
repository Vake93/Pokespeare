import axios from 'axios';

const baseURL = process.env.REACT_APP_BASE_URL ? process.env.REACT_APP_BASE_URL : 'http://localhost:5070/api/v1/';

const pokespeareApi = axios.create({
    baseURL,
});

export default pokespeareApi;