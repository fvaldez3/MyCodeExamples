import axios from 'axios';
import { API_HOST_PREFIX, onGlobalSuccess, onGlobalError } from './serviceHelpers';

const endPoint = `${API_HOST_PREFIX}/api/licenses`;
const getById = (licenseId) => {
    const config = {
        method: 'GET',
        url: `${endPoint}/${licenseId}`,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

export const getAll = (pageIndex, pageSize) => {
    const config = {
        method: 'GET',
        url: `${endPoint}/paginate?pageIndex=${pageIndex}&pageSize=${pageSize}`,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const getByCreated = (createdBy, pageIndex, pageSize) => {
    const config = {
        method: 'GET',
        url: `${endPoint} /createdby?pageIndex=${pageIndex}&pageSize=${pageSize}&CreatedBy=${createdBy}`,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const getByLicenseTypeId = (licenseType, pageIndex, pageSize) => {
    const config = {
        method: 'Get',
        url: `${endPoint} /licensetype?pageIndex=${pageIndex}&pageSize=${pageSize}&LicenseTypeId=${licenseType}`,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const add = (payload) => {
    const config = {
        method: 'POST',
        url: `${endPoint}`,
        withCredentials: true,
        data: payload,
        crossdomain: true,
        header: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const update = (payload) => {
    const config = {
        method: 'PUT',
        url: `${endPoint}/${payload.id}`,
        withCredentials: true,
        data: payload,
        crossdomain: true,
        header: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const deleteLicense = (licenseId) => {
    const config = {
        method: 'GET',
        url: `${endPoint}/${licenseId}`,
        withCredentials: true,
        crossdomain: true,
        header: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

export const getAllExpired = (pageIndex, pageSize) => {
    const config = {
        method: 'GET',
        url: `${endPoint}/paginate/expired?pageIndex=${pageIndex}&pageSize=${pageSize}`,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

export const getAllAboutToExpire = (pageIndex, pageSize, daysToExpire) => {
    const config = {
        method: 'GET',
        url: `${endPoint}/paginate/abouttoexpire?pageIndex=${pageIndex}&pageSize=${pageSize}&daysToExpire=${daysToExpire}`,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const getDetailedById = (licenseId) => {
    const config = {
        method: 'GET',
        url: `${endPoint}/related/${licenseId}`,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const sendExpiredNotification = (payload) => {
    const config = {
        method: 'POST',
        url: `${endPoint}/notification/expired`,
        data: payload,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const sendAboutToExpireNotification = (payload) => {
    const config = {
        method: 'POST',
        url: `${endPoint}/notification/abouttoexpire`,
        data: payload,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const search = (pageIndex, pageSize, query) => {
    const config = {
        method: 'GET',
        url: `${endPoint}/search?pageindex=${pageIndex}&pagesize=${pageSize}&query=${query}`,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const getByLicenseState = (pageIndex, pageSize, state) => {
    const config = {
        method: 'Get',
        url: `${endPoint}/state?pageIndex=${pageIndex}&pageSize=${pageSize}&state=${state}`,
        withCredentials: true,
        crossdomain: true,
        headers: { 'Content-Type': 'application/json' },
    };
    return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const LicenseService = {
    getById,
    getAll,
    getByCreated,
    getByLicenseTypeId,
    add,
    update,
    deleteLicense,
    getAllExpired,
    getDetailedById,
    sendExpiredNotification,
    getAllAboutToExpire,
    sendAboutToExpireNotification,
    search,
    getByLicenseState,
};
export default LicenseService;
