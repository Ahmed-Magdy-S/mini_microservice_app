import {useState, useEffect} from "react";
import axios from "axios"
import CommentCreate from "./CommentCreate"
import CommentList from "./CommentList"
const PostList = ()=>{
    const [posts,setPosts] = useState({});
    
    const fetchPosts = async ()=>{
        const res = await axios.get("http://localhost:4002/posts");
        console.log(res.data)
        setPosts(res.data);
    }
    
    useEffect(()=>{
        fetchPosts();
    },[])
    
    const renderedPosts = Object.values(posts).map(post=>{
        return (
            <div key={post.id} className="card" style={{width:"30%",marginBottom:"20px"}}>
                <div className="card-body">
                    <h3>title: {post.title}</h3>
                    <CommentList comments={post.comments} />
                    <CommentCreate postId={post.id} />
                </div>
            </div>
        )
    });
    
    return(
        <>
            <div>{renderedPosts}</div>
        </>
    )
    
}

export  default  PostList