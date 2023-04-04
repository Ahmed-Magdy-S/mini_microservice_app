import PostCreate from "./PostCreate"
import PostList from "./PostList"

function App() {
    return (
        <> 
            <div className="container">
                <h1>Create Post</h1>
                <PostCreate/>
                <hr/>
                <h2>Posts</h2>
                <PostList/>
            </div>
        </>
    );
}

export default App;
