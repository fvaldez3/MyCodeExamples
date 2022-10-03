import axios from 'axios';
import { API_HOST_PREFIX, onGlobalError, onGlobalSuccess } from './serviceHelpers';
import debug from 'sabio-debug';

const serviceEndpoints = {
    locationEndpoint: `${API_HOST_PREFIX}/api/locations`,
    lookUpEndpoint: `${API_HOST_PREFIX}/api/lookups`,
};
const _logger = debug.extend('LocationForm').extend('LocationService');

const getStates = () => {
    _logger(`${serviceEndpoints.lookUpEndpoint}/states`);
    const config = {
        method: 'GET',
        url: `${serviceEndpoints.lookUpEndpoint}/states`,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const getLocationTypes = (payload) => {
    _logger(`${serviceEndpoints.lookUpEndpoint}`);
    const config = {
        method: 'POST',
        url: serviceEndpoints.lookUpEndpoint,
        data: payload,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const addLocation = (payload) => {
    const config = {
        method: 'POST',
        url: serviceEndpoints.locationEndpoint,
        data: payload,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const updateLocation = (payload) => {
    _logger(payload);
    const config = {
        method: 'PUT',
        url: serviceEndpoints.locationEndpoint + `/${payload.id}`,
        data: payload,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const deleteLocation = (id) => {
    const config = {
        method: 'DELETE',
        url: serviceEndpoints.locationEndpoint + `/${id}`,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const getLocationsFromCurrentUser = (pageIndex, pageSize) => {
    const config = {
        method: 'GET',
        url: serviceEndpoints.locationEndpoint + `/createdby?pageIndex=${pageIndex}&pageSize=${pageSize}`,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };

    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const locationService = {
    getStates,
    getLocationTypes,
    addLocation,
    getLocationsFromCurrentUser,
    updateLocation,
    deleteLocation,
};

export default locationService;
