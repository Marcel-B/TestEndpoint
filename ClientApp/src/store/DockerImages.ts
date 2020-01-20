import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface DockerImagesState {
    isLoading: boolean;
    startDateIndex?: number;
    dockerImages: DockerImage[];
}

export interface DockerImage {
    created: string;
    updated: string;
    name: string;
    tag: string;
    pusher: string;
    owner: string;
    repoName: string;
    id: string;
    updateId: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestDockerImagesAction {
    type: 'REQUEST_DOCKER_IMAGES';
    startDateIndex: number;
}

interface ReceiveDockerImagesAction {
    type: 'RECEIVE_DOCKER_IMAGES';
    startDateIndex: number;
    dockerImages: DockerImage[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestDockerImagesAction | ReceiveDockerImagesAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestDockerImages: (startDateIndex: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.dockerImages && startDateIndex !== appState.dockerImages.startDateIndex) {
            fetch(`api/docker`)
                .then(response => response.json() as Promise<DockerImage[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_DOCKER_IMAGES', startDateIndex: startDateIndex, dockerImages: data });
                });

            dispatch({ type: 'REQUEST_DOCKER_IMAGES', startDateIndex: startDateIndex });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: DockerImagesState = { dockerImages: [], isLoading: false };

export const reducer: Reducer<DockerImagesState> = (state: DockerImagesState | undefined, incomingAction: Action): DockerImagesState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_DOCKER_IMAGES':
            return {
                startDateIndex: action.startDateIndex,
                dockerImages: state.dockerImages,
                isLoading: true
            };
        case 'RECEIVE_DOCKER_IMAGES':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            if (action.startDateIndex === state.startDateIndex) {
                return {
                    startDateIndex: action.startDateIndex,
                    dockerImages: action.dockerImages,
                    isLoading: false
                };
            }
            break;
    }
    return state;
};
